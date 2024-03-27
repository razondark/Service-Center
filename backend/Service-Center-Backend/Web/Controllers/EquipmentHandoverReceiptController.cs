using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Controllers
{
	[Route("api/ehr")]
	[ApiController]
	public class EquipmentHandoverReceiptController : ControllerBase
	{
		private readonly IEquipmentHandoverReceipt _equipmentHandoverReceipt;

		public EquipmentHandoverReceiptController(IEquipmentHandoverReceipt equipmentHandoverReceipt)
		{
			_equipmentHandoverReceipt = equipmentHandoverReceipt;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllEHR()
		{
			return await _equipmentHandoverReceipt.GetAllEHR();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEHRById(int id)
		{
			return await _equipmentHandoverReceipt.GetEHRById(id);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateEHR([FromBody] EquipmentHandoverReceiptDto deviceDto)
		{
			return await _equipmentHandoverReceipt.CreateEHR(deviceDto);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateEHR([FromBody] EquipmentHandoverReceiptDto deviceDto)
		{
			return await _equipmentHandoverReceipt.UpdateEHR(deviceDto);
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteEHR(int id)
		{
			return await _equipmentHandoverReceipt.DeleteEHR(id);
		}
	}
}
