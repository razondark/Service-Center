using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class DevicePartProvider
{
	[Key]
	public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<DevicePartDelivery> DevicePartDeliveries { get; set; } = new List<DevicePartDelivery>();
}
