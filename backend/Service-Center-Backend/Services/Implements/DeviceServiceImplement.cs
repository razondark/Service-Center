using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Center_Backend.Context;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Dto.Handlers;
using Service_Center_Backend.Web.Mappers;

namespace Service_Center_Backend.Services.Implements
{
	public class DeviceServiceImplement : IDeviceService
	{
		private readonly ServiceCenterContext _context;

		public DeviceServiceImplement(ServiceCenterContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> CreateDevice(DeviceDto deviceDto)
		{
			try
			{
				deviceDto.Id = default(int);
				_context.Devices.Add(DeviceMapper.ToModel(deviceDto));
				await _context.SaveChangesAsync();

				return new ObjectResult(deviceDto) { StatusCode = StatusCodes.Status201Created };
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}

		public async Task<IActionResult> DeleteDevice(int id)
		{
			try
			{
				var device = await _context.Devices.Where(d => d.Id == id).FirstOrDefaultAsync();
				if (device is null)
				{
					return new NotFoundObjectResult(new NotFoundExceptionHandler("Устройство не найдено"));
				}

				_context.Devices.Remove(device);
				await _context.SaveChangesAsync();

				return new OkObjectResult(device);
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}

		public async Task<IActionResult> GetAllDevices()
		{
			var devices = await _context.Devices.ToListAsync();
			if (devices is null || devices.Count == 0)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Устройства не найдены"));
			}

			return new OkObjectResult(DeviceMapper.ToDto(devices));
		}

		public async Task<IActionResult> GetDeviceById(int id)
		{
			var device = await _context.Devices.Where(d => d.Id == id).FirstOrDefaultAsync();
			if (device is null)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Устройство не найдено"));
			}

			return new OkObjectResult(DeviceMapper.ToDto(device));
		}

		public async Task<IActionResult> UpdateDevice(DeviceDto deviceDto)
		{
			try
			{
				_context.Devices.Update(DeviceMapper.ToModel(deviceDto));
				await _context.SaveChangesAsync();

				return new OkObjectResult(deviceDto);
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}
	}
}
