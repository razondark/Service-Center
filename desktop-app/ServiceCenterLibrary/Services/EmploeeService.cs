using ServiceCenterLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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

		public async Task<EmployeeDto?> CreateAsync(EmployeeDto employee)
		{
			var response = await _httpClient.PostAsJsonAsync<EmployeeDto>(_config.CreateEmployeeLink, employee);

			return await response.Content.ReadFromJsonAsync<EmployeeDto>();
		}

		public async Task<EmployeeDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteEmployeeLink}/{id}");

			return await response.Content.ReadFromJsonAsync<EmployeeDto>();
		}

		public async Task<IEnumerable<EmployeeDto>?> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeDto>>(_config.GetAllEmployeesLink);
		}

		public async Task<EmployeeDto?> UpdateAsync(EmployeeDto employee)
		{
			var response = await _httpClient.PutAsJsonAsync<EmployeeDto>(_config.CreateClientLink, employee);

			return await response.Content.ReadFromJsonAsync<EmployeeDto>();
		}
	}
}
