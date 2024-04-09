using ServiceCenterLibrary.Dto;
using System.Net.Http;
using System.Net.Http.Json;

namespace ServiceCenterLibrary.Services
{
    public static class AccountService
    {
        public static async Task<AccountsResponse> GetAccounts()
        {
            using (var client = new HttpClient())
            {
                var account = await client.GetFromJsonAsync<AccountsResponse>($"http://localhost:5095/api/accounts");

                return account!;
            }
        }


    }
}