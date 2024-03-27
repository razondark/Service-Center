namespace Service_Center_Backend.Web.Dto.Response
{
	public class DevicesResponse
	{
		public List<DeviceDto> Devices { get; set; } = null!;

		public DevicesResponse(List<DeviceDto> devices)
		{
			Devices = devices;
		}
	}
}
