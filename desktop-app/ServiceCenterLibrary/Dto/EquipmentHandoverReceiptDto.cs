using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	[DisplayName("Акты приема-передачи")]
	public class EquipmentHandoverReceiptDto : IDto
	{
		[DisplayName("Номер")]
		public int Id { get; set; }

		[DisplayName("Клиент")]
		public int IdClient { get; set; }

		[DisplayName("Устройство")]
		public int IdDevice { get; set; }

		[DisplayName("Сотрудник")]
		public int IdEmployee { get; set; }

		[DisplayName("Дата приема")]
		public DateTime? EquipmentAcceptanceDate { get; set; }

		[DisplayName("Дата выдачи")]
		public DateTime? EquipmentIssueDate { get; set; }

		[DisplayName("Описание неисправности")]
		public string? DefectDescription { get; set; }
	}
}
