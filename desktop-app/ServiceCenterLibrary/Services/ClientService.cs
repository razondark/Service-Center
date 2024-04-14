using ServiceCenterLibrary.Config;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Exceptions;
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
			var response = await _httpClient.GetAsync(_config.GetAllClientsLink);
			return await HandleResponseAsync<IEnumerable<ClientDto>>(response);
		}

		public async Task<ClientDto?> CreateAsync(ClientDto client)
		{
			var json = JsonSerializer.Serialize(client);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(_config.CreateClientLink, content);

			return await HandleResponseAsync<ClientDto>(response);
		}

		public async Task<ClientDto?> UpdateAsync(ClientDto client)
		{
			var json = JsonSerializer.Serialize(client);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(_config.UpdateClientLink, content);

			return await HandleResponseAsync<ClientDto>(response);
		}

		public async Task<ClientDto?> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_config.DeleteClientLink}/{id}");

			return await HandleResponseAsync<ClientDto>(response);
		}
	}
}
