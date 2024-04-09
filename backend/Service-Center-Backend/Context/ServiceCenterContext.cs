using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Service_Center_Backend.Models;
using Service_Center_Backend.Views;

namespace Service_Center_Backend.Context;

public partial class ServiceCenterContext : DbContext
{
    public ServiceCenterContext()
    {
    }

    public ServiceCenterContext(DbContextOptions<ServiceCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientsAndDevicesView> ClientsAndDevicesViews { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DevicePart> DeviceParts { get; set; }

    public virtual DbSet<DevicePartDelivery> DevicePartDeliveries { get; set; }

    public virtual DbSet<DevicePartProvider> DevicePartProviders { get; set; }

    public virtual DbSet<DevicePartUsed> DevicePartUseds { get; set; }

    public virtual DbSet<DevicesOnStorageView> DevicesOnStorageViews { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EquipmentHandoverReceipt> EquipmentHandoverReceipts { get; set; }

    public virtual DbSet<PopularRepairPartsView> PopularRepairPartsViews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceWorkDto> ServiceWorks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //=> optionsBuilder.UseNpgsql("Host=195.80.51.63;Port=5555;Database=service-center;Username=postgres;Password=root");
	{
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.ToTable("account");

            entity.HasIndex(e => e.Login, "account_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<ClientsAndDevicesView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("clients_and_devices_view");

            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.Imei)
                .HasMaxLength(15)
                .HasColumnName("imei");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(50)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Model)
                .HasMaxLength(75)
                .HasColumnName("model");
            entity.Property(e => e.NumberOfTimesInRepair).HasColumnName("Number of times in repair");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_pkey");

            entity.ToTable("device");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeviceType)
                .HasMaxLength(30)
                .HasColumnName("device_type");
            entity.Property(e => e.Imei)
                .HasMaxLength(15)
                .HasColumnName("imei");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(50)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Model)
                .HasMaxLength(75)
                .HasColumnName("model");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(30)
                .HasColumnName("serial_number");
        });

        modelBuilder.Entity<DevicePart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_part_pkey");

            entity.ToTable("device_part");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.InventoryStock).HasColumnName("inventory_stock");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(150)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.WarrantyDuration)
                .HasDefaultValue(0)
                .HasColumnName("warranty_duration");
        });

        modelBuilder.Entity<DevicePartDelivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_part_delivery_pkey");

            entity.ToTable("device_part_delivery");

            entity.HasIndex(e => new { e.IdDevicePartProvider, e.IdDevicePart, e.DeliveryDate }, "uq_dpd_providerdevicepartdate").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("delivery_date");
            entity.Property(e => e.IdDevicePart).HasColumnName("id_device_part");
            entity.Property(e => e.IdDevicePartProvider).HasColumnName("id_device_part_provider");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdDevicePartNavigation).WithMany(p => p.DevicePartDeliveries)
                .HasForeignKey(d => d.IdDevicePart)
                .HasConstraintName("fk_dpd_iddevicepart");

            entity.HasOne(d => d.IdDevicePartProviderNavigation).WithMany(p => p.DevicePartDeliveries)
                .HasForeignKey(d => d.IdDevicePartProvider)
                .HasConstraintName("fk_dpd_iddevicepartprovider");
        });

        modelBuilder.Entity<DevicePartProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_part_provider_pkey");

            entity.ToTable("device_part_provider");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .HasColumnName("company_name");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Inn)
                .HasMaxLength(20)
                .HasColumnName("inn");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<DevicePartUsed>(entity =>
        {
            entity.HasKey(e => new { e.IdServiceWork, e.IdDevicePart }).HasName("pk_devicepartused_idserviceworkiddevicepart");

            entity.ToTable("device_part_used");

            entity.Property(e => e.IdServiceWork).HasColumnName("id_service_work");
            entity.Property(e => e.IdDevicePart).HasColumnName("id_device_part");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdDevicePartNavigation).WithMany(p => p.DevicePartUseds)
                .HasForeignKey(d => d.IdDevicePart)
                .HasConstraintName("fk_devicepartused_iddevicepart");

            entity.HasOne(d => d.IdServiceWorkNavigation).WithMany(p => p.DevicePartUseds)
                .HasForeignKey(d => d.IdServiceWork)
                .HasConstraintName("fk_devicepartused_idservicework");
        });

        modelBuilder.Entity<DevicesOnStorageView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("devices_on_storage_view");

            entity.Property(e => e.DeviceType)
                .HasMaxLength(30)
                .HasColumnName("device_type");
            entity.Property(e => e.EquipmentAcceptanceDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("equipment_acceptance_date");
            entity.Property(e => e.Imei)
                .HasMaxLength(15)
                .HasColumnName("imei");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(50)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Model)
                .HasMaxLength(75)
                .HasColumnName("model");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(30)
                .HasColumnName("serial_number");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.Passport)
                .HasMaxLength(30)
                .HasColumnName("passport");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("phone_number");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
        });

        modelBuilder.Entity<EquipmentHandoverReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("equipment_handover_receipt_pkey");

            entity.ToTable("equipment_handover_receipt");

            entity.HasIndex(e => new { e.IdClient, e.IdDevice, e.IdEmployee, e.EquipmentAcceptanceDate }, "uq_ehr_clientdeviceemployeeacceptancedate").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DefectDescription)
                .HasDefaultValueSql("'Отсутствует'::text")
                .HasColumnName("defect_description");
            entity.Property(e => e.EquipmentAcceptanceDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("equipment_acceptance_date");
            entity.Property(e => e.EquipmentIssueDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("equipment_issue_date");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdDevice).HasColumnName("id_device");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.EquipmentHandoverReceipts)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("fk_idclient");

            entity.HasOne(d => d.IdDeviceNavigation).WithMany(p => p.EquipmentHandoverReceipts)
                .HasForeignKey(d => d.IdDevice)
                .HasConstraintName("fk_iddevice");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EquipmentHandoverReceipts)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("fk_idemployee");
        });

        modelBuilder.Entity<PopularRepairPartsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("popular_repair_parts_view");

            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(150)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.UsesCount).HasColumnName("Uses count");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pkey");

            entity.ToTable("service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'Отсутствует'::text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ServiceWorkDto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_work_pkey");

            entity.ToTable("service_work");

            entity.HasIndex(e => e.IdEquipmentHandoverReceipt, "uq_servicework_idehr").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdEquipmentHandoverReceipt).HasColumnName("id_equipment_handover_receipt");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.ServiceWorks)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_servicework_idemployee");

            entity.HasOne(d => d.IdEquipmentHandoverReceiptNavigation).WithOne(p => p.ServiceWork)
                .HasForeignKey<ServiceWorkDto>(d => d.IdEquipmentHandoverReceipt)
                .HasConstraintName("fk_servicework_idehr");

            entity.HasMany(d => d.IdServices).WithMany(p => p.IdServiceWorks)
                .UsingEntity<Dictionary<string, object>>(
                    "ServiceProvided",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("IdService")
                        .HasConstraintName("fk_serviceprovided_idservice"),
                    l => l.HasOne<ServiceWorkDto>().WithMany()
                        .HasForeignKey("IdServiceWork")
                        .HasConstraintName("fk_serviceprovided_idservicework"),
                    j =>
                    {
                        j.HasKey("IdServiceWork", "IdService").HasName("pk_serviceprovided_idserviceworkidservice");
                        j.ToTable("service_provided");
                        j.IndexerProperty<int>("IdServiceWork").HasColumnName("id_service_work");
                        j.IndexerProperty<int>("IdService").HasColumnName("id_service");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
