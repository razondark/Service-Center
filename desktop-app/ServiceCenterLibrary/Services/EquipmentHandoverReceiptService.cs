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
	public class EquipmentHandoverReceiptService : BaseService, IDataService<EquipmentHandoverReceiptDto>
	{
		private static EquipmentHandoverReceiptService? _instance;

		public static EquipmentHandoverReceiptService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EquipmentHandoverReceiptService();
				}
				return _instance;
			}
		}

		public async Task<EquipmentHandoverReceiptDto?> CreateAsync(EquipmentHandoverReceiptDto ehr)
		{
			var json = JsonSerializer.Serialize(ehr);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(_config.CreateEHRLink, content);

			return await HandleResponseAsync<EquipmentHandoverReceiptDto>(response);
		}

		public async Task<EquipmentHandoverReceiptDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteEHRLink}/{id}");

			return await HandleResponseAsync<EquipmentHandoverReceiptDto>(response);
		}

		public async Task<IEnumerable<EquipmentHandoverReceiptDto>?> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_config.GetAllEHRLink);
			return await HandleResponseAsync<IEnumerable<EquipmentHandoverReceiptDto>>(response);
		}

		public async Task<EquipmentHandoverReceiptDto?> GetByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync($"{_config.GetEHRByIdLink}/{id}");
			return await HandleResponseAsync<EquipmentHandoverReceiptDto>(response);
		}

		public async Task<EquipmentHandoverReceiptDto?> UpdateAsync(EquipmentHandoverReceiptDto ehr)
		{
			var json = JsonSerializer.Serialize(ehr);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(_config.UpdateEHRLink, content);

			return await HandleResponseAsync<EquipmentHandoverReceiptDto>(response);
		}
	}
}
