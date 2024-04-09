using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	public class AccountsResponse
	{
		public List<AccountDto> Accounts { get; set; } = null!;

		public AccountsResponse(List<AccountDto> accounts)
		{
			Accounts = accounts;
		}
	}
}
