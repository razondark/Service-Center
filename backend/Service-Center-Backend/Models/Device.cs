using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class Device
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string DeviceType { get; set; } = null!;

    public string Imei { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public virtual ICollection<EquipmentHandoverReceipt> EquipmentHandoverReceipts { get; set; } = new List<EquipmentHandoverReceipt>();
}
