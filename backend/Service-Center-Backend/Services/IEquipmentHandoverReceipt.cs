using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Services
{
	public interface IEquipmentHandoverReceipt
	{
		public Task<IActionResult> GetAllEHR();

		public Task<IActionResult> GetEHRById(int id);

		public Task<IActionResult> CreateEHR(EquipmentHandoverReceiptDto ehr);

		public Task<IActionResult> UpdateEHR(EquipmentHandoverReceiptDto ehr);

		public Task<IActionResult> DeleteEHR(int id);
	}
}
