using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using Service_Center_Backend.Context;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Dto.Authentication;
using Service_Center_Backend.Web.Handlers;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Service_Center_Backend.Services.Implements
{
    public class AccountServiceImplements : IAccountService
    {
        private readonly ServiceCenterContext _context;

        public AccountServiceImplements(ServiceCenterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            if (accounts is null || accounts.Count == 0)
            {
                return new NotFoundObjectResult(new NotFoundExceptionHandler("Аккаунты не найдены"));
            }

            return new OkObjectResult(new AccountsResponse(accounts));
        }

        public async Task<IActionResult> CreateAccount(Account account)
        {
            try
            {
                account.Password = HashPassword(account.Password);

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return new ObjectResult(account) { StatusCode = StatusCodes.Status201Created };
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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                var sb = new StringBuilder(capacity: bytes.Length);
                foreach (var i in bytes)
                {
                    sb.Append(i.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _context.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (account is null)
            {
                return new NotFoundObjectResult(new NotFoundExceptionHandler("Аккаунт не найден"));
            }

            return new OkObjectResult(account);
        }

        public async Task<IActionResult> Login(AuthenticationRequest loginRequest)
        {
            var account = await _context.Accounts.Where(a => a.Login.ToLower().Equals(loginRequest.Login.ToLower())).FirstOrDefaultAsync();
            if (account is null)
            {
                return new NotFoundObjectResult(new NotFoundExceptionHandler("Некорректный логин"));
            }

            if (!account.Password.Equals(HashPassword(loginRequest.Password)))
            {
                return new NotFoundObjectResult(new NotFoundExceptionHandler("Некорректный пароль"));
            }

            return new OkObjectResult(account);
        }

        public async Task<IActionResult> UpdateAccount(Account account)
        {
            try
            {
                account.Password = HashPassword(account.Password);

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return new OkObjectResult(account);
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

        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var account = await _context.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (account is null)
                {
                    return new NotFoundObjectResult(new NotFoundExceptionHandler("Аккаунт не найден"));
                }

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return new OkObjectResult(account);
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
