using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class DevicePartDelivery
{
	[Key]
	public int Id { get; set; }

    public int IdDevicePartProvider { get; set; }

    public int IdDevicePart { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Quantity { get; set; }

    public virtual DevicePart IdDevicePartNavigation { get; set; } = null!;

    public virtual DevicePartProvider IdDevicePartProviderNavigation { get; set; } = null!;
}
