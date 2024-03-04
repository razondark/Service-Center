using System;
using System.Collections.Generic;

namespace CourseProject.Models;

public partial class DevicePartUsed
{
    public int IdServiceWork { get; set; }

    public int IdDevicePart { get; set; }

    public int Quantity { get; set; }

    public virtual DevicePart IdDevicePartNavigation { get; set; } = null!;

    public virtual ServiceWork IdServiceWorkNavigation { get; set; } = null!;
}
