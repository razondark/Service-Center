using Service_Center_Backend.Models;

namespace Service_Center_Backend.Web.Dto.Response
{
    public class AccountsResponse
    {
        public List<Account> Accounts { get; set; } = null!;

        public AccountsResponse(List<Account> accounts)
        {
            Accounts = accounts;
        }
    }
}
