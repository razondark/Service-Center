using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Dto.Authentication;

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

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] AuthenticationRequest loginRequest)
		{
			return await _accountService.Login(loginRequest);
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
