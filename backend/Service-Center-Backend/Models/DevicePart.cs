﻿using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class DevicePart
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public decimal Cost { get; set; }

    public int? WarrantyDuration { get; set; }

    public int InventoryStock { get; set; }

    public virtual ICollection<DevicePartDelivery> DevicePartDeliveries { get; set; } = new List<DevicePartDelivery>();

    public virtual ICollection<DevicePartUsed> DevicePartUseds { get; set; } = new List<DevicePartUsed>();
}
