using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Mappers
{
	public static class EquipmentHandoverReceiptMapper
	{
		public static EquipmentHandoverReceiptDto ToDto(EquipmentHandoverReceipt ehr)
		{
			return new EquipmentHandoverReceiptDto()
			{
				Id = ehr.Id,
				IdClient = ehr.IdClient,
				IdDevice = ehr.IdDevice,
				IdEmployee = ehr.IdEmployee,
				EquipmentAcceptanceDate = ehr.EquipmentAcceptanceDate,
				EquipmentIssueDate = ehr.EquipmentIssueDate,
				DefectDescription = ehr.DefectDescription
			};
		}

		public static List<EquipmentHandoverReceiptDto> ToDto(IList<EquipmentHandoverReceipt> ehrs)
		{
			var listEHRDto = new List<EquipmentHandoverReceiptDto>(ehrs.Count);

			foreach (var ehr in ehrs)
			{
				listEHRDto.Add(ToDto(ehr));
			}

			return listEHRDto;
		}

		public static EquipmentHandoverReceipt ToModel(EquipmentHandoverReceiptDto ehrDto)
		{
			return new EquipmentHandoverReceipt()
			{
				Id = ehrDto.Id,
				IdClient = ehrDto.IdClient,
				IdDevice = ehrDto.IdDevice,
				IdEmployee = ehrDto.IdEmployee,
				EquipmentAcceptanceDate = ehrDto.EquipmentAcceptanceDate,
				EquipmentIssueDate = ehrDto.EquipmentIssueDate,
				DefectDescription = ehrDto.DefectDescription
			};
		}
	}
}
