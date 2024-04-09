using ServiceCenterLibrary.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Services
{
	public class BaseService
	{
		protected readonly AppConfig _config = new AppConfig();
		protected readonly HttpClient _httpClient = new HttpClient();
	}
}
