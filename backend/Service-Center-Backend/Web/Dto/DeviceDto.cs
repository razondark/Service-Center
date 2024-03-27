namespace Service_Center_Backend.Web.Dto
{
	public class DeviceDto
	{
		public int Id { get; set; }

		public string Manufacturer { get; set; } = null!;

		public string Model { get; set; } = null!;

		public string DeviceType { get; set; } = null!;

		public string Imei { get; set; } = null!;

		public string? SerialNumber { get; set; }
	}
}
