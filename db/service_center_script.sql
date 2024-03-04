CREATE DATABASE Service_Center;

CREATE DOMAIN PHONE AS varchar(12);
CREATE DOMAIN EMAIL AS varchar(50);
CREATE DOMAIN FULL_NAME AS varchar(150);
CREATE DOMAIN IMEI AS varchar(15);

CREATE TABLE Client(
	id SERIAL PRIMARY KEY,
	full_name  FULL_NAME NOT NULL,
	phone_number PHONE NOT NULL,
	email EMAIL NULL CHECK (email ILIKE '%@%.%')
);

CREATE TABLE Device(
	id SERIAL PRIMARY KEY,
	manufacturer varchar(50) NOT NULL,
	model varchar(75) NOT NULL,
	device_type varchar(30) NOT NULL,
	imei IMEI NOT NULL,
	serial_number varchar(30) NULL
);

CREATE TABLE Employee(
	id SERIAL PRIMARY KEY,
	full_name FULL_NAME,
	passport varchar(30) NOT NULL CHECK (passport ~ '^\d{4} \d{6}$'),
	phone_number PHONE NOT NULL,
	email EMAIL NULL CHECK (email ILIKE '%@%.%'),
	position varchar(50) NOT NULL
);

CREATE TABLE Equipment_Handover_Receipt(
	id SERIAL PRIMARY KEY,
	id_client INT NOT NULL,
	id_device INT NOT NULL,
	id_employee INT NOT NULL,
	equipment_acceptance_date TIMESTAMP DEFAULT NOW(),
	equipment_issue_date TIMESTAMP NULL,
	defect_description TEXT DEFAULT 'Отсутствует',
	
	CONSTRAINT UQ_EHR_ClientDeviceEmployeeAcceptancedate UNIQUE(id_client, id_device, id_employee, equipment_acceptance_date),
	
	CONSTRAINT FK_IdClient FOREIGN KEY (id_client) REFERENCES Client (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_IdDevice FOREIGN KEY (id_device) REFERENCES Device (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_IdEmployee FOREIGN KEY (id_employee) REFERENCES Employee (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Device_Part_Provider(
	id SERIAL PRIMARY KEY,
	company_name varchar(150) NOT NULL,
	INN varchar(20) NOT NULL,
	address varchar(150) NOT NULL,
	phone_number PHONE NOT NULL,
	email EMAIL NOT NULL CHECK (email ILIKE '%@%.%')
);

CREATE TABLE Device_Part(
	id SERIAL PRIMARY KEY,
	name varchar(150) NOT NULL,
	manufacturer varchar(150) NOT NULL,
	cost MONEY NOT NULL,
	warranty_duration INT DEFAULT 0,
	inventory_stock INT NOT NULL
);

CREATE TABLE Device_Part_Delivery(
	id SERIAL PRIMARY KEY,
	id_device_part_provider INT NOT NULL,
	id_device_part INT NOT NULL,
	delivery_date TIMESTAMP DEFAULT NOW(),
	quantity INT NOT NULL,
	
	CONSTRAINT UQ_DPD_ProviderDevicepartDate UNIQUE(id_device_part_provider, id_device_part, delivery_date),
	CONSTRAINT FK_DPD_idDevicePartProvider FOREIGN KEY (id_device_part_provider) REFERENCES Device_Part_Provider (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_DPD_IdDevicePart FOREIGN KEY (id_device_part) REFERENCES Device_Part (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Service(
	id SERIAL PRIMARY KEY,
	name varchar(100) NOT NULL,
	description TEXT DEFAULT 'Отсутствует',
	cost MONEY NOT NULL
);

CREATE TABLE Service_Work(
	id SERIAL PRIMARY KEY,
	id_equipment_handover_receipt INT NOT NULL,
	id_employee INT NOT NULL,
	start_date TIMESTAMP DEFAULT NOW(),
	end_date TIMESTAMP NULL,
	cost MONEY NULL,
	
	CONSTRAINT UQ_ServiceWork_IdEHR UNIQUE(id_equipment_handover_receipt),
	
	CONSTRAINT FK_ServiceWork_IdEHR FOREIGN KEY (id_equipment_handover_receipt) REFERENCES Equipment_Handover_Receipt (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_ServiceWork_IdEmployee FOREIGN KEY (id_employee) REFERENCES Employee (id) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE Device_Part_Used(
	id_service_work INT NOT NULL,
	id_device_part INT NOT NULL,
	quantity INT NOT NULL DEFAULT 1,
	
	CONSTRAINT PK_DevicePartUsed_IdserviceworkIddevicepart PRIMARY KEY(id_service_work, id_device_part),

	CONSTRAINT FK_DevicePartUsed_IdServiceWork FOREIGN KEY (id_service_work) REFERENCES Service_Work (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_DevicePartUsed_IdDevicePart FOREIGN KEY (id_device_part) REFERENCES Device_Part (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Service_Provided(
	id_service_work INT NOT NULL,
	id_service INT NOT NULL,
	
	CONSTRAINT PK_ServiceProvided_IdserviceworkIdservice PRIMARY KEY(id_service_work, id_service),

	CONSTRAINT FK_ServiceProvided_IdServiceWork FOREIGN KEY (id_service_work) REFERENCES Service_Work (id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_ServiceProvided_IdService FOREIGN KEY (id_service) REFERENCES Service (id) ON DELETE CASCADE ON UPDATE CASCADE
);




CREATE VIEW Clients_and_devices_view
AS
	SELECT Client.full_name, Client.phone_number, Device.manufacturer, Device.model, Device.imei, COUNT(*) "Number of times in repair"
	FROM Equipment_Handover_Receipt EHR
	JOIN Client ON EHR.id_client = Client.id
	JOIN Device ON Device.id = Client.id
	GROUP BY Client.full_name, Client.phone_number, Device.manufacturer, Device.model, Device.imei;
	
CREATE VIEW Devices_on_storage_view
AS
	SELECT Device.manufacturer, Device.model, Device.device_type, Device.imei, Device.serial_number, EHR.equipment_acceptance_date
	FROM Equipment_Handover_Receipt EHR
	JOIN Service_Work ON Service_Work.id_equipment_handover_receipt = EHR.id
	JOIN Device ON Device.id = EHR.id_device
	WHERE Service_Work.end_date IS NOT NULL AND EHR.equipment_issue_date IS NULL;
	
CREATE VIEW Popular_repair_parts_view
AS
	SELECT Part.name, Part.manufacturer, Part.cost, COUNT(*) "Uses count"
	FROM Device_Part_Used
	JOIN Device_Part Part ON Part.id = Device_Part_Used.id_device_part
	GROUP BY Part.name, Part.manufacturer, Part.cost, Device_Part_Used.quantity
	ORDER BY Device_Part_Used.quantity DESC;
	
	
	
	
	
CREATE PROCEDURE Calculate_total_cost(id_work INT)
AS
$$
DECLARE
    cost_services money;
    cost_parts money;
BEGIN
	cost_parts := COALESCE((SELECT SUM(Device_Part.cost * DPU.quantity)
						FROM Device_Part_Used DPU
						JOIN Device_Part ON Device_Part.id = DPU.id_device_part
						WHERE DPU.id_service_work = id_work), CAST(0 as money));
	
	cost_services := COALESCE((SELECT SUM(Service.cost)
						FROM Service_Provided SP
						JOIN Service ON Service.id = SP.id_service
						WHERE SP.id_service_work = id_work), CAST(0 as money));
	
	IF (SELECT COUNT(SP.id_service_work)
		FROM Service_Provided SP
		WHERE id_service_work = id_work) > 1 OR cost_parts > CAST(0 as money) THEN
		cost_services := cost_services - (SELECT Service.cost
								FROM Service
								WHERE Service.name ILIKE '%диагностика%');
	END IF;
	
	UPDATE Service_Work
	SET cost = (cost_parts + cost_services)
	WHERE id = id_work;
END;
$$
LANGUAGE plpgsql;

CREATE PROCEDURE Start_Working(id_equipment_handover_receipt INT, id_employee INT)
AS
$$
BEGIN
	INSERT INTO Service_Work (id_equipment_handover_receipt, id_employee)
	VALUES
		(id_equipment_handover_receipt, id_employee);
END;
$$
LANGUAGE plpgsql;




CREATE OR REPLACE FUNCTION trg_InsertServiceWork()
RETURNS TRIGGER AS 
$$
DECLARE
    id_diagnostics int;
BEGIN
    id_diagnostics := (SELECT id
						FROM Service
						WHERE Service.name ILIKE '%диагностика%');

    INSERT INTO Service_Provided
    VALUES (NEW.id, id_diagnostics);

    RETURN NEW;
END;
$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trg_InsertInsertServiceWork
	AFTER INSERT ON Service_Work
	FOR EACH STATEMENT	
	EXECUTE FUNCTION trg_InsertServiceWork();


CREATE OR REPLACE FUNCTION trg_InsertService()
RETURNS TRIGGER AS 
$$
BEGIN
    IF EXISTS (SELECT 1 
				FROM Service
				WHERE Service.name ILIKE '%диагностика%') THEN
		INSERT INTO Service
		SELECT * 
		FROM NEW
		WHERE NEW.name NOT ILIKE '%диагностика%';
	ELSE
		INSERT INTO Service
        SELECT *
        FROM NEW;
	END IF;
END;
$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trg_InsertService
	BEFORE INSERT OR UPDATE ON Service
	FOR EACH ROW
	EXECUTE FUNCTION trg_InsertService();


CREATE OR REPLACE FUNCTION trg_InsertDevicePartUsed()
RETURNS TRIGGER AS 
$$
BEGIN
    UPDATE Device_Part
    SET inventory_stock = inventory_stock - NEW.quantity
    WHERE id = NEW.id_device_part;
    RETURN NEW;
END;
$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trg_InsertDevicePartUsed
	AFTER INSERT OR UPDATE ON Device_Part_Used
	FOR EACH ROW
	EXECUTE FUNCTION trg_InsertDevicePartUsed();


CREATE OR REPLACE FUNCTION trg_UpdateServiceWorkCheckDate()
RETURNS TRIGGER AS 
$$
BEGIN
    IF NEW.start_date > NEW.end_date THEN
        RAISE EXCEPTION 'Дата начала работы должна быть раньше даты окончания работы.';
    END IF;

    RETURN NEW;
END;
$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trg_UpdateServiceWorkCheckDate
	AFTER INSERT OR UPDATE ON Service_Work
	FOR EACH ROW
	EXECUTE FUNCTION trg_UpdateServiceWorkCheckDate();
	
	
	
	
	
INSERT INTO Client
VALUES
	(1, 'Иванов Иван Иванович', '89084497798', 'ivanov@mail.ru'),
	(2, 'Петров Петр Петрович', '89964350019', 'petrov@yandex.com'),
	(3, 'Сидорова Анна Александровна', '89782485547', 'sidorova@yandex.com'),
	(4, 'Смирнова Елена Викторовна', '89248181109', 'smirnova@yandex.com'),
	(5, 'Кузнецов Дмитрий Владимирович', '89387776302', 'kuznetsov@gmail.com'),
	(6, 'Васильева Ольга Алексеевна', '89081545413', 'vasilieva@gmail.com'),
	(7, 'Морозов Сергей Викторович', '89583708560', 'morozov@gmail.com'),
	(8, 'Новикова Мария Петровна', '89089529309', 'novikova@gmail.com'),
	(9, 'Алексеев Алексей Алексеевич', '89034924794', 'alekseev@mail.ru'),
	(10, 'Федорова Екатерина Игоревна', '89505246800', 'fedorova@mail.ru');

INSERT INTO Device
VALUES
 (1, 'Apple', 'iPhone 12', 'Смартфон', '123456789012345', 'SN123456'),
 (2, 'Samsung', 'Galaxy S21', 'Смартфон', '234567890123456', 'SN234567'),
 (3, 'Xiaomi', 'Redmi Note 10', 'Смартфон', '345678901234567', 'SN345678'),
 (4, 'Huawei', 'P40 Pro', 'Смартфон', '456789012345678', 'SN456789'),
 (5, 'OnePlus', '9 Pro', 'Смартфон', '567890123456789', 'SN567890'),
 (6, 'Google', 'Pixel 5', 'Смартфон', '678901234567890', 'SN678901'),
 (7, 'Sony', 'Xperia 1 III', 'Смартфон', '789012345678901', 'SN789012'),
 (8, 'LG', 'Wing', 'Смартфон', '890123456789012', 'SN890123'),
 (9, 'Motorola', 'Razr 5G', 'Смартфон', '901234567890123', 'SN901234'),
 (10, 'Nokia', '8.3 5G', 'Смартфон', '012345678901234', 'SN012345');
 
INSERT INTO Employee
VALUES
 (1, 'Щукин Николай Геннадиевич', '1234 567890', '9876543210', 'shnik@service.net', 'Мастер'),
 (2, 'Копылов Руслан Валерьевич', '2345 678901', '9876543211', 'rukop@service.net', 'Инженер'),
 (3, 'Савин Константин Лаврентьевич', '3456 789012', '9876543212', 'kosav@service.net', 'Техник'),
 (4, 'Мартынов Георгий Евсеевич', '4567 890123', '9876543213', 'geomar@service.net', 'Сервисный инженер'),
 (5, 'Афанасьев Мечеслав Рубенович', '5678 901234', '9876543214', 'mechaf@service.net', 'Паяльщик'),
 (6, 'Попов Донат Евгеньевич', '6789 012345', '9876543215', 'dopop@service.net', 'Техник'),
 (7, 'Давыдов Феликс Ростиславович', '7890 123456', '9876543216', 'fedal@service.net', 'Мастер'),
 (8, 'Миронова Леся Кирилловна', '8901 234567', '9876543217', 'lemir@service.net', 'Кассир'),
 (9, 'Михайлова Элина Михаиловна', '9012 345678', '9876543218', 'elmi@service.net', 'Менеджер'),
 (10, 'Владимирова Полина Степановна', '0123 456789', '9876543219', 'povla@service.net', 'Кассир');