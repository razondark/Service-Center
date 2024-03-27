namespace Service_Center_Backend.Web.Dto
{
	public class ClientDto
	{
		public int Id { get; set; }

		public string FullName { get; set; } = null!;

		public string PhoneNumber { get; set; } = null!;

		public string? Email { get; set; }
	}
}
