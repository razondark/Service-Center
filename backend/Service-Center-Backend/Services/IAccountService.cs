using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Dto.Authentication;

namespace Service_Center_Backend.Services
{
	public interface IAccountService
	{
		public Task<IActionResult> GetAllAccounts();

		public Task<IActionResult> GetAccountById(int id);

		public Task<IActionResult> Login(AuthenticationRequest loginRequest);

		public Task<IActionResult> CreateAccount(Account account);

		public Task<IActionResult> UpdateAccount(Account account);

		public Task<IActionResult> DeleteAccount(int id);
	}
}
