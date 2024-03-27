using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Services
{
	public interface IDeviceService
	{
		public Task<IActionResult> GetAllDevices();

		public Task<IActionResult> GetDeviceById(int id);

		public Task<IActionResult> CreateDevice(DeviceDto deviceDto);

		public Task<IActionResult> UpdateDevice(DeviceDto deviceDto);

		public Task<IActionResult> DeleteDevice(int id);
	}
}
