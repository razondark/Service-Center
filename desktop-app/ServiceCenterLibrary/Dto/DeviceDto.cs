using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto
{
	[DisplayName("Устройства")]
	public class DeviceDto : IDto
	{
		[DisplayName("Код")]
		public int Id { get; set; }

		[DisplayName("Производитель")]
		public string Manufacturer { get; set; } = null!;

		[DisplayName("Модель")]
		public string Model { get; set; } = null!;

		[DisplayName("Тип")]
		public string DeviceType { get; set; } = null!;

		[DisplayName("IMEI")]
		public string Imei { get; set; } = null!;

		[DisplayName("Серийный номер")]
		public string? SerialNumber { get; set; }
	}
}
