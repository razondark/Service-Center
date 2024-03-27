namespace Service_Center_Backend.Web.Dto
{
	public class EquipmentHandoverReceiptDto
	{
		public int Id { get; set; }

		public int IdClient { get; set; }

		public int IdDevice { get; set; }

		public int IdEmployee { get; set; }

		public DateTime? EquipmentAcceptanceDate { get; set; }

		public DateTime? EquipmentIssueDate { get; set; }

		public string? DefectDescription { get; set; }
	}
}
