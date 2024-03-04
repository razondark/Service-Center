using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class Employee
{
	[Key]
	public int Id { get; set; }

    public string? FullName { get; set; }

    public string Passport { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string Position { get; set; } = null!;

    public virtual ICollection<EquipmentHandoverReceipt> EquipmentHandoverReceipts { get; set; } = new List<EquipmentHandoverReceipt>();

    public virtual ICollection<ServiceWork> ServiceWorks { get; set; } = new List<ServiceWork>();
}
