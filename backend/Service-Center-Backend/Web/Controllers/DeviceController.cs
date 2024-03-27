using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Controllers
{
	[Route("api/devices")]
	[ApiController]
	public class DeviceController : ControllerBase
	{
		private readonly IDeviceService _deviceService;

		public DeviceController(IDeviceService deviceService)
		{
			_deviceService = deviceService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllDevices()
		{
			return await _deviceService.GetAllDevices();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetDeviceById(int id)
		{
			return await _deviceService.GetDeviceById(id);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateDevice([FromBody] DeviceDto deviceDto)
		{
			return await _deviceService.CreateDevice(deviceDto);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateDevice([FromBody] DeviceDto deviceDto)
		{
			return await _deviceService.UpdateDevice(deviceDto);
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteDevice(int id)
		{
			return await _deviceService.DeleteDevice(id);
		}
	}
}
