using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Mappers
{
	public static class DeviceMapper
	{
		public static DeviceDto ToDto(Device device)
		{
			return new DeviceDto()
			{
				Id = device.Id,
				Manufacturer = device.Manufacturer,
				Model = device.Model,
				DeviceType = device.DeviceType,
				Imei = device.Imei,
				SerialNumber = device.SerialNumber
			};
		}

		public static List<DeviceDto> ToDto(IList<Device> devices)
		{
			var listDeviceDto = new List<DeviceDto>(devices.Count);

			foreach (var device in devices)
			{
				listDeviceDto.Add(ToDto(device));
			}

			return listDeviceDto;
		}

		public static Device ToModel(DeviceDto deviceDto)
		{
			return new Device()
			{
				Id = deviceDto.Id,
				Manufacturer = deviceDto.Manufacturer,
				Model = deviceDto.Model,
				DeviceType = deviceDto.DeviceType,
				Imei = deviceDto.Imei,
				SerialNumber = deviceDto.SerialNumber
			};
		}
	}
}
