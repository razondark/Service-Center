namespace Service_Center_Backend.Web.Dto.Response
{
	public class EquipmentHandoverReceiptResponse
	{
		public List<EquipmentHandoverReceiptDto> EquipmentHandoverReceipts { get; set; } = null!;

		public EquipmentHandoverReceiptResponse(List<EquipmentHandoverReceiptDto> ehr)
		{
			EquipmentHandoverReceipts = ehr;
		}
	}
}
