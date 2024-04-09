using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	public class AccountDto
	{
		public int Id { get; set; }

		public string Login { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string? Email { get; set; }

		public string Status { get; set; } = null!;
	}
}
