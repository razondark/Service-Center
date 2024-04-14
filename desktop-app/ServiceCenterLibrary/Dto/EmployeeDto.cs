using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	[DisplayName("Сотрудники")]
	public class EmployeeDto : IDto
	{
		[DisplayName("Код")]
		public int Id { get; set; }

		[DisplayName("ФИО")]
		public string FullName { get; set; } = null!;

		[DisplayName("Паспорт")]
		public string Passport { get; set; } = null!;

		[DisplayName("Телефон")]
		public string PhoneNumber { get; set; } = null!;

		[DisplayName("Электронная почта")]
		public string? Email { get; set; }

		[DisplayName("Должность")]
		public string Position { get; set; } = null!;
	}
}
