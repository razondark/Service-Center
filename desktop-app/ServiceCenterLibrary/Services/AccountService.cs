using Microsoft.Windows.Themes;
using ServiceCenterLibrary.Dto;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ServiceCenterLibrary.Services
{
    public class AccountService : BaseService//, IDataService<AccountDto>
	{
		private static AccountService? _instance;

		public static AccountService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AccountService();
				}
				return _instance;
			}
		}

		private string HashPassword(string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

				var sb = new StringBuilder(capacity: bytes.Length);
				foreach (var i in bytes)
				{
					sb.Append(i.ToString("x2"));
				}

				return sb.ToString();
			}
		}

		private class LoginRequest
		{
			public string? Login { get; set; }
			public string? Password { get; set; }
		}

		public async Task<AccountDto?> LoginAsync(string login, string password)
		{
			var request = new LoginRequest();
			request.Login = login;
			request.Password = HashPassword(password);

			var json = JsonSerializer.Serialize(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("http://localhost:5000/api/accounts/login", content);

			return await HandleResponseAsync<AccountDto>(response);
		}
	}
}