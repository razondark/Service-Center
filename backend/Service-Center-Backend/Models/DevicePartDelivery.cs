using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class DevicePartDelivery
{
    public int Id { get; set; }

    public int IdDevicePartProvider { get; set; }

    public int IdDevicePart { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Quantity { get; set; }

    public virtual DevicePart IdDevicePartNavigation { get; set; } = null!;

    public virtual DevicePartProvider IdDevicePartProviderNavigation { get; set; } = null!;
}
