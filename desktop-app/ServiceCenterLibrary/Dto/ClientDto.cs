using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	[DisplayName("Клиенты")]
	public class ClientDto
	{
		[DisplayName("Идентификатор")]
		public int Id { get; set; }

		[DisplayName("ФИО")]
		public string FullName { get; set; } = null!;

		[DisplayName("Номер телефона")]
		public string PhoneNumber { get; set; } = null!;

		[DisplayName("Электронная почта")]
		public string? Email { get; set; }
	}
}
