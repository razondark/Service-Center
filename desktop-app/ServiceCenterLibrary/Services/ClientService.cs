using ServiceCenterLibrary.Config;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Dto.Response;
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

		public async Task<IEnumerable<ClientDto>?> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<ClientDto>>(_config.GetAllClientsLink);
		}

		public async Task<ClientDto?> CreateAsync(ClientDto client)
		{
			var response = await _httpClient.PostAsJsonAsync<ClientDto>(_config.CreateClientLink, client);

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}

		public async Task<ClientDto?> UpdateAsync(ClientDto client)
		{
			var response = await _httpClient.PutAsJsonAsync<ClientDto>(_config.CreateClientLink, client);

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}

		public async Task<ClientDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteClientLink}/{id}");

			return await response.Content.ReadFromJsonAsync<ClientDto>();
		}
	}
}
