using System;
using System.Collections.Generic;

namespace CourseProject.Views;

public partial class ClientsAndDevicesView
{
    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? Imei { get; set; }

    public long? NumberOfTimesInRepair { get; set; }
}
