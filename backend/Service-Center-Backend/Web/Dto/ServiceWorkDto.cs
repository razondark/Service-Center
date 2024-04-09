using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class ServiceWorkDto
{
    public int Id { get; set; }

    public int IdEquipmentHandoverReceipt { get; set; }

    public int IdEmployee { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Cost { get; set; }

    public virtual ICollection<DevicePartUsed> DevicePartUseds { get; set; } = new List<DevicePartUsed>();

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual EquipmentHandoverReceipt IdEquipmentHandoverReceiptNavigation { get; set; } = null!;

    public virtual ICollection<Service> IdServices { get; set; } = new List<Service>();
}
