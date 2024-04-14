using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Center_Backend.Context;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto.Handlers;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Mappers;

namespace Service_Center_Backend.Services.Implements
{
	public class EquipmentHandoverReceiptImplement : IEquipmentHandoverReceipt
	{
		private readonly ServiceCenterContext _context;

		public EquipmentHandoverReceiptImplement(ServiceCenterContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> CreateEHR(EquipmentHandoverReceiptDto ehrDto)
		{
			try
			{
				ehrDto.Id = default(int);
				_context.EquipmentHandoverReceipts.Add(EquipmentHandoverReceiptMapper.ToModel(ehrDto));
				await _context.SaveChangesAsync();

				return new ObjectResult(ehrDto) { StatusCode = StatusCodes.Status201Created };
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}

		public async Task<IActionResult> DeleteEHR(int id)
		{
			try
			{
				var ehr = await _context.EquipmentHandoverReceipts.Where(e => e.Id == id).FirstOrDefaultAsync();
				if (ehr is null)
				{
					return new NotFoundObjectResult(new NotFoundExceptionHandler("Акт не найден"));
				}

				_context.EquipmentHandoverReceipts.Remove(ehr);
				await _context.SaveChangesAsync();

				return new OkObjectResult(EquipmentHandoverReceiptMapper.ToDto(ehr));
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}

		public async Task<IActionResult> GetAllEHR()
		{
			var ehrs = await _context.EquipmentHandoverReceipts.ToListAsync();
			if (ehrs is null || ehrs.Count == 0)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Акты не найдены"));
			}

			return new OkObjectResult(EquipmentHandoverReceiptMapper.ToDto(ehrs));
		}

		public async Task<IActionResult> GetEHRById(int id)
		{
			var ehr = await _context.EquipmentHandoverReceipts.Where(e => e.Id == id).FirstOrDefaultAsync();
			if (ehr is null)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Акт не найден"));
			}

			return new OkObjectResult(EquipmentHandoverReceiptMapper.ToDto(ehr));
		}

		public async Task<IActionResult> UpdateEHR(EquipmentHandoverReceiptDto ehrDto)
		{
			try
			{
				_context.EquipmentHandoverReceipts.Update(EquipmentHandoverReceiptMapper.ToModel(ehrDto));
				await _context.SaveChangesAsync();

				return new OkObjectResult(ehrDto);
			}
			catch (DbUpdateException ex)
			{
				return new ConflictObjectResult(new BaseException(ex.InnerException is not null ? ex.InnerException.Message : ex.Message));
			}
			catch (Exception ex)
			{
				return new ObjectResult(ex);
			}
		}
	}
}
