using ServiceCenterLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Services
{
	public class EmploeeService : BaseService, IDataService<EmployeeDto>
	{
		private static EmploeeService? _instance;

		public static EmploeeService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EmploeeService();
				}
				return _instance;
			}
		}

		public async Task<IEnumerable<EmployeeDto>?> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_config.GetAllEmployeesLink);
			return await HandleResponseAsync<IEnumerable<EmployeeDto>>(response);
		}

		public async Task<EmployeeDto?> CreateAsync(EmployeeDto employee)
		{
			var json = JsonSerializer.Serialize(employee);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(_config.CreateEmployeeLink, content);

			return await HandleResponseAsync<EmployeeDto>(response);
		}

		public async Task<EmployeeDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteEmployeeLink}/{id}");

			return await HandleResponseAsync<EmployeeDto>(response);
		}

		public async Task<EmployeeDto?> UpdateAsync(EmployeeDto employee)
		{
			var json = JsonSerializer.Serialize(employee);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(_config.UpdateEmployeeLink, content);

			return await HandleResponseAsync<EmployeeDto>(response);
		}
	}
}
