using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            return await _accountService.GetAllAccounts();
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetAccountById(int id)
		{
			return await _accountService.GetAccountById(id);
		}

		[HttpGet("login")]
		public async Task<IActionResult> GetAccountByLoginAndPassword(string login, string password)
		{
			return await _accountService.GetAccountByLoginAndPassword(login, password);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateAccount([FromBody] Account account)
		{
			return await _accountService.CreateAccount(account);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateAccount([FromBody] Account account)
		{
			return await _accountService.UpdateAccount(account);
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			return await _accountService.DeleteAccount(id);
		}
	}
}
