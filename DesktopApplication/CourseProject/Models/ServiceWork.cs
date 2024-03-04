using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class ServiceWork
{
	[Key]
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
