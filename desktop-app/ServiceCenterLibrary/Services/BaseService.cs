using ServiceCenterLibrary.Config;
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
	public class BaseService
	{
		protected readonly AppConfig _config = new AppConfig();
		protected readonly HttpClient _httpClient = new HttpClient();

		protected async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				string message = await response.Content.ReadAsStringAsync();
				throw new ExceptionHandler(message);
			}
			//var f = await response.Content.ReadAsStringAsync();
			return await response.Content.ReadFromJsonAsync<T>();
		}
	}
}
