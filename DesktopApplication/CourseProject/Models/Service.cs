using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public partial class Service
{
	[Key]
	public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public virtual ICollection<ServiceWork> IdServiceWorks { get; set; } = new List<ServiceWork>();
}
