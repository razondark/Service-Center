CREATE DATABASE Service_Center;

CREATE DOMAIN PHONE AS varchar(12);
CREATE DOMAIN EMAIL AS varchar(50);
CREATE DOMAIN FULL_NAME AS varchar(150);
CREATE DOMAIN IMEI AS varchar(15);

CREATE TABLE Account(
	id SERIAL PRIMARY KEY,
	login varchar(100) NOT NULL UNIQUE,
	password varchar(150) NOT NULL,
	email EMAIL NULL CHECK (email ILIKE '%@%.%'),
	status varchar(50) NOT NULL
);

INSERT INTO Account
VALUES
	(1, 'razondark', 'db8023f8cb3221526188e5f0e0e879499ae24dbb517b775125e0f2802f4fa284', 'razon@dark.net', 'admin');

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
 
INSERT INTO Equipment_Handover_Receipt
VALUES
 (1, 1, 2, 8, '2023-05-01T10:00:00', '2023-05-03T15:30:00', 'Не работает дисплей'),
 (2, 2, 3, 9, '2023-05-02T09:15:00', '2023-05-04T11:45:00', 'Проблемы с зарядкой'),
 (3, 3, 4, 10, '2023-05-03T14:30:00', '2023-05-06T09:00:00', 'Не работает камера'),
 (4, 2, 5, 8, '2023-05-04T11:00:00', NULL, 'Сломана кнопка включения'),
 (5, 5, 6, 9, '2023-05-05T13:45:00', '2023-05-07T16:20:00', 'Проблемы с сетью'),
 (6, 6, 7, 10, '2023-05-06T16:30:00', NULL, 'Быстро разряжается батарея'),
 (7, 7, 8, 8, '2023-05-07T09:10:00', '2023-05-09T14:15:00', 'Не работает микрофон'),
 (8, 3, 9, 9, '2023-05-08T12:20:00', NULL, 'Проблемы с динамиком'),
 (9, 4, 10, 10, '2023-05-09T15:50:00', '2023-05-11T10:30:00', 'Не загружается операционная система'),
 (10, 8, 1, 8, '2023-05-10T11:40:00', '2023-05-12T13:55:00', 'Битый экран'),
 (11, 1, 2, 8, '2023-05-11T09:30:00', NULL, 'Проблемы с батареей'),
 (12, 3, 3, 9, '2023-05-12T14:20:00', '2023-05-14T10:00:00', 'Не работает камера'),
 (13, 6, 4, 10, '2023-05-13T11:10:00', '2023-05-16T09:30:00', 'Проблемы с сетью'),
 (14, 9, 5, 8, '2023-05-14T13:20:00', '2023-05-16T15:45:00', 'Сломан разъем для наушников'),
 (15, 10, 6, 9, '2023-05-15T16:30:00', NULL, 'Быстро разряжается батарея'),
 (16, 7, 7, 10, '2023-05-16T09:50:00', '2023-05-18T12:10:00', 'Не работает микрофон'),
 (17, 5, 8, 8, '2023-05-17T12:40:00', NULL, 'Проблемы с динамиком'),
 (18, 9, 9, 9, '2023-05-18T15:15:00', '2023-05-20T10:20:00', 'Не загружается операционнаясистема'),
 (19, 10, 10, 10, '2023-05-19T10:25:00', '2023-05-21T14:30:00', 'Битый экран'),
 (20, 8, 1, 8, '2023-05-20T14:50:00', NULL, 'Проблемы с батареей');
 
INSERT INTO Device_Part_Provider
VALUES
	(1, 'ООО "ТехноПартнер"', '1234567890', 'г. Москва, ул. Центральная, 10', '81234567890', 'info@technopartner.com'),
	(2, 'АО "ЭлектроСистема"', '9876543210', 'г. Санкт-Петербург, пр. Солнечный, 5', '89876543210', 'info@electrosystem.com'),
	(3, 'ИП "МобильТех"', '5678901234', 'г. Екатеринбург, ул. Свободная, 7', '85678901234', 'info@mobiletech.com'),
	(4, 'ОАО "Техносервис"', '4321098765', 'г. Новосибирск, пр. Ленина, 15', '84321098765', 'info@technoservice.com'),
	(5, 'ФГУП "Электроника"', '0987654321', 'г. Казань, ул. Пушкина, 20', '80987654321', 'info@electronics.com'),
	(6, 'ООО "Технофорум"', '3456789012', 'г. Ростов-на-Дону, пр. Мира, 12', '83456789012', 'info@technoforum.com'),
	(7, 'АО "МобильМастер"', '7890123456', 'г. Самара, ул. Садовая, 25', '87890123456', 'info@mobilemaster.com'),
	(8, 'ИП "СервисЭксперт"', '2109876543', 'г. Уфа, пр. Гагарина, 8', '82109876543', 'info@serviceexpert.com'),
	(9, 'ОАО "ТехноГрупп"', '6543210987', 'г. Волgоград, ул. Новая, 3', '86543210987', 'info@technogroup.com'),
	(10, 'АО "ЭлектроСервис"', '8765432109', 'г. Омск, пр. Звездный, 6', '88765432109', 'info@electroservice.com');
	
INSERT INTO Device_Part
VALUES
	(1, 'Батарея', 'Samsung', 2000.00, 12, 50),
	(2, 'Дисплей', 'Apple', 5000.00, 6, 30),
	(3, 'Камера', 'Sony', 4000.00, NULL, 10),
	(4, 'Шлейф', 'LG', 1500.00, NULL, 10),
	(5, 'Кнопка питания', 'Nokia', 200.00, NULL, 10),
	(6, 'Антенна', 'Huawei', 400.00, NULL, 5),
	(7, 'Слуховой динамик', 'Motorola', 650.00, NULL, 7),
	(8, 'Микрофон', 'Xiaomi', 1750.00, 3, 3),
	(9, 'Taptic engine', 'Apple', 5000.00, NULL, 2),
	(10, 'Корпус', 'HTC', 7000.00, NULL, 9),
	(11, 'Микрофон', 'Apple', 2000.00, NULL, 7),
	(12, 'Сенсорный экран', 'Google', 8000.00, 12, 16),
	(13, 'Зарядное устройство', 'OnePlus', 500.00, NULL, 15),
	(14, 'Аккумулятор', 'Sony', 1500.00, 6, 66),
	(15, 'Кнопка громкости', 'LG', 300.00, NULL, 12);
	
INSERT INTO Device_Part_Delivery
VALUES
	(1, 1, 1, '2023-05-01T10:00:00', 20),
	(2, 2, 3, '2023-05-02T11:30:00', 15),
	(3, 3, 5, '2023-05-03T14:45:00', 10),
	(4, 4, 7, '2023-05-04T09:15:00', 5),
	(5, 5, 9, '2023-05-05T16:20:00', 8),
	(6, 6, 11, '2023-05-06T13:00:00', 12),
	(7, 7, 13, '2023-05-07T11:45:00', 25),
	(8, 8, 15, '2023-05-08T10:30:00', 18),
	(9, 9, 2, '2023-05-09T14:00:00', 10),
	(10, 10, 4, '2023-05-10T15:45:00', 15),
	(11, 1, 6, '2023-05-11T09:30:00', 20),
	(12, 2, 8, '2023-05-12T12:15:00', 15),
	(13, 3, 10, '2023-05-13T14:30:00', 10),
	(14, 4, 12, '2023-05-14T11:00:00', 5),
	(15, 5, 14, '2023-05-15T16:45:00', 8),
	(16, 6, 1, '2023-05-16T13:30:00', 12),
	(17, 7, 3, '2023-05-17T10:45:00', 25),
	(18, 8, 5, '2023-05-18T09:30:00', 18),
	(19, 9, 7, '2023-05-19T12:00:00', 10),
	(20, 10, 9, '2023-05-20T14:15:00', 15);
	
INSERT INTO Service
VALUES
	(1, 'Замена экрана', 'Замена поврежденного экрана устройства', 1500.00),
	(2, 'Установка программного обеспечения', 'Установка необходимых программ на устройство', 800.00),
	(3, 'Диагностика неисправностей', 'Определение причин и устранение неисправностей устройства', 500.00),
	(4, 'Замена аккумулятора', 'Замена старого аккумулятора на новый', 1200.00),
	(5, 'Восстановление данных', 'Восстановление утерянных или поврежденных данных', 1000.00),
	(6, 'Чистка от пыли и грязи', 'Очистка устройства от пыли и грязи', 400.00),
	(7, 'Замена кнопок', 'Замена поврежденных или изношенных кнопок устройства', 600.00),
	(8, 'Ремонт разъемов', 'Ремонт поврежденных разъемов устройства', 900.00),
	(9, 'Настройка сетевых параметров', 'Настройка сетевых параметров для правильного функционирования устройства', 700.00),
	(10, 'Удаление вирусов', 'Обнаружение и удаление вредоносного программного обеспечения', 1500.00),
	(11, 'Замена задней крышки', 'Замена поврежденной или треснутой задней крышки устройства', 800.00),
	(12, 'Настройка операционной системы', 'Настройка операционной системы для оптимальной работы устройства', 1000.00),
	(13, 'Замена камеры', 'Замена поврежденной или неисправной камеры устройства', 1100.00),
	(14, 'Ремонт звуковой системы', 'Ремонт поврежденной звуковой системы устройства', 950.00),
	(15, 'Установка защитного стекла', 'Установка защитного стекла на экран устройства', 500.00);
	
INSERT INTO Service_Work
VALUES
	(1, 1, 1, '2023-05-03T09:00:00', '2023-05-03T12:00:00', 1500.00),
	(2, 2, 3, '2023-05-04T10:30:00', '2023-05-04T13:30:00', 800.00),
	(3, 3, 2, '2023-05-05T13:00:00', '2023-05-05T16:00:00', 500.00),
	(4, 4, 5, '2023-05-06T11:30:00', '2023-05-06T14:30:00', 1200.00),
	(5, 5, 4, '2023-05-07T14:00:00', '2023-05-07T17:00:00', 1000.00),
	(6, 6, 6, '2023-05-08T15:30:00', '2023-05-08T18:30:00', 400.00),
	(7, 7, 7, '2023-05-09T10:00:00', '2023-05-09T13:00:00', 600.00),
	(8, 8, 3, '2023-05-10T12:30:00', '2023-05-10T15:30:00', 900.00),
	(9, 9, 4, '2023-05-11T15:00:00', '2023-05-11T18:00:00', 700.00),
	(10, 10, 1, '2023-05-12T11:00:00', '2023-05-12T14:00:00', 1500.00),
	(11, 11, 1, '2023-05-13T13:30:00', '2023-05-13T16:30:00', 800.00),
	(12, 12, 2, '2023-05-14T16:00:00', '2023-05-14T19:00:00', 1000.00),
	(13, 13, 4, '2023-05-15T09:30:00', '2023-05-15T12:30:00', 1100.00),
	(14, 14, 6, '2023-05-16T12:00:00', '2023-05-16T15:00:00', 950.00),
	(15, 15, 6, '2023-05-17T14:30:00', '2023-05-17T17:30:00', 500.00),
	(16, 16, 5, '2023-05-18T11:30:00', '2023-05-18T14:30:00', 1500.00),
	(17, 17, 2, '2023-05-19T13:00:00', '2023-05-19T16:00:00', 800.00),
	(18, 18, 3, '2023-05-20T15:30:00', '2023-05-20T18:30:00', 1000.00),
	(19, 19, 1, '2023-05-21T10:00:00', '2023-05-21T13:00:00', 1100.00),
	(20, 20, 4, '2023-05-22T12:30:00', '2023-05-22T15:30:00', 950.00);
	
INSERT INTO Device_Part_Used
VALUES
	(1, 2),
	(1, 14),
	(3, 3),
	(4, 5),
	(6, 1),
	(10, 12),
	(11, 14),
	(12, 3),
	(13, 14),
	(15, 14),
	(18, 12),
	(19, 12),
	(20, 14);

INSERT INTO Service_Provided
VALUES
	(1, 1),
	(1, 4),
	(2, 8),
	(2, 15),
	(3, 13),
	(4, 7),
	(6, 4),
	(7, 6),
	(7, 14),
	(7, 15),
	(7, 8),
	(8, 14),
	(9, 12),
	(9, 5),
	(10, 1),
	(11, 4),
	(12, 13),
	(13, 4),
	(13, 9),
	(15, 4),
	(15, 6),
	(17, 6),
	(18, 2),
	(19, 1),
	(20, 4),
	(20, 10);






	
	
	
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
	FOR EACH ROW	
	EXECUTE FUNCTION trg_InsertServiceWork();


CREATE OR REPLACE FUNCTION trg_InsertService()
RETURNS TRIGGER AS 
$$
BEGIN
    IF EXISTS (SELECT 1 
				FROM Service
				WHERE Service.name ILIKE '%диагностика%') AND NEW.name ILIKE '%диагностика%' THEN
		RETURN NULL;
	ELSE
		RETURN NEW;
	END IF;
	
	RETURN NEW;
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
	
	
	
