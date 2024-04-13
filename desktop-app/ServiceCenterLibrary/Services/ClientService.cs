using ServiceCenterLibrary.Config;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Dto.Response;
using ServiceCenterLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Services
{
    public class ClientService : BaseService, IDataService<ClientDto>
	{
		private static ClientService? _instance;

		public static ClientService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ClientService();
				}
				return _instance;
			}
		}

		private async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				string message = await response.Content.ReadAsStringAsync();
				throw new ExceptionHandler(message);
			}

			return await response.Content.ReadFromJsonAsync<T>();
		}

		public async Task<IEnumerable<ClientDto>?> GetAllAsync()
		{
			HttpResponseMessage response = await _httpClient.GetAsync(_config.GetAllClientsLink);
			return await HandleResponseAsync<IEnumerable<ClientDto>>(response);
		}

		public async Task<ClientDto?> CreateAsync(ClientDto client)
		{
			var response = await _httpClient.PostAsJsonAsync<ClientDto>(_config.CreateClientLink, client);

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}

		public async Task<ClientDto?> UpdateAsync(ClientDto client)
		{
			var response = await _httpClient.PutAsJsonAsync<ClientDto>(_config.UpdateClientLink, client);

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}

		public async Task<ClientDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteClientLink}/{id}");

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}
	}
}
