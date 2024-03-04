using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class EquipmentHandoverReceipt
{
	[Key]
	public int Id { get; set; }

    public int IdClient { get; set; }

    public int IdDevice { get; set; }

    public int IdEmployee { get; set; }

    public DateTime? EquipmentAcceptanceDate { get; set; }

    public DateTime? EquipmentIssueDate { get; set; }

    public string? DefectDescription { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Device IdDeviceNavigation { get; set; } = null!;

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual ServiceWork? ServiceWork { get; set; }
}
