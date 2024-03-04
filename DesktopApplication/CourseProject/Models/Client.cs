using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class Client
{
    [Key]
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<EquipmentHandoverReceipt> EquipmentHandoverReceipts { get; set; } = new List<EquipmentHandoverReceipt>();
}
