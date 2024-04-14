using ServiceCenterLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Services
{
	public class DeviceService : BaseService, IDataService<DeviceDto>
	{
		private static DeviceService? _instance;

		public static DeviceService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DeviceService();
				}
				return _instance;
			}
		}

		public async Task<IEnumerable<DeviceDto>?> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_config.GetAllDevicesLink);
			return await HandleResponseAsync<IEnumerable<DeviceDto>>(response);
		}

		public async Task<DeviceDto?> CreateAsync(DeviceDto device)
		{
			var json = JsonSerializer.Serialize(device);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(_config.CreateDeviceLink, content);

			return await HandleResponseAsync<DeviceDto>(response);
		}

		public async Task<DeviceDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteDeviceLink}/{id}");

			return await HandleResponseAsync<DeviceDto>(response);
		}

		public async Task<DeviceDto?> UpdateAsync(DeviceDto device)
		{
			var json = JsonSerializer.Serialize(device);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(_config.UpdateDeviceLink, content);

			return await HandleResponseAsync<DeviceDto>(response);
		}
	}
}
