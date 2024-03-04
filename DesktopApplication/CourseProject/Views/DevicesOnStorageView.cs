using System;
using System.Collections.Generic;

namespace CourseProject.Views;

public partial class DevicesOnStorageView
{
    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? DeviceType { get; set; }

    public string? Imei { get; set; }

    public string? SerialNumber { get; set; }

    public DateTime? EquipmentAcceptanceDate { get; set; }
}
