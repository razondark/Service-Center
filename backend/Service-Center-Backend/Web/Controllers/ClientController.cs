using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Controllers
{
	[Route("api/clients")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly IClientService _clientService;

		public ClientController(IClientService clientService) 
		{
			_clientService = clientService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllClients()
		{
			return await _clientService.GetAllClients();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClientById(int id)
		{
			return await _clientService.GetClientById(id);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateClient([FromBody] ClientDto client)
		{
			return await _clientService.CreateClient(client);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateClient([FromBody] ClientDto client)
		{
			return await _clientService.UpdateClient(client);
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteClient(int id)
		{
			return await _clientService.DeleteClient(id);
		}
	}
}
