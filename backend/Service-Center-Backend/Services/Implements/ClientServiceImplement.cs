using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Center_Backend.Context;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Dto.Handlers;
using Service_Center_Backend.Web.Dto.Response;
using Service_Center_Backend.Web.Mappers;
using System.Security.Principal;

namespace Service_Center_Backend.Services.Implements
{
	public class ClientServiceImplement : IClientService
	{
		private readonly ServiceCenterContext _context;

		public ClientServiceImplement(ServiceCenterContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> CreateClient(ClientDto clientDto)
		{
			try
			{
				_context.Clients.Add(ClientMapper.ToModel(clientDto));
				await _context.SaveChangesAsync();

				return new ObjectResult(clientDto) { StatusCode = StatusCodes.Status201Created };
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

		public async Task<IActionResult> DeleteClient(int id)
		{
			try
			{
				var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
				if (client is null)
				{
					return new NotFoundObjectResult(new NotFoundExceptionHandler("Клиент не найден"));
				}

				_context.Clients.Remove(client);
				await _context.SaveChangesAsync();

				return new OkObjectResult(client);
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

		public async Task<IActionResult> GetAllClients()
		{
			var clients = await _context.Clients.ToListAsync();
			if (clients is null || clients.Count == 0)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Клиенты не найдены"));
			}

			return new OkObjectResult(ClientMapper.ToDto(clients));
		}

		public async Task<IActionResult> GetClientById(int id)
		{
			var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
			if (client is null)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Клиент не найден"));
			}

			return new OkObjectResult(ClientMapper.ToDto(client));
		}

		public async Task<IActionResult> UpdateClient(ClientDto clientDto)
		{
			try
			{
				_context.Clients.Update(ClientMapper.ToModel(clientDto));
				await _context.SaveChangesAsync();

				return new OkObjectResult(clientDto);
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
