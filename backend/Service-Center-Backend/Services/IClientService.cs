using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Services
{
    public interface IClientService
    {
        public Task<IActionResult> GetAllClients();

        public Task<IActionResult> GetClientById(int id);

        public Task<IActionResult> CreateClient(ClientDto client);

        public Task<IActionResult> UpdateClient(ClientDto client);

        public Task<IActionResult> DeleteClient(int id);
    }
}
