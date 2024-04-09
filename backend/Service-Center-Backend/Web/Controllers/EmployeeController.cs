using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Services;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Controllers
{
	[Route("api/employees")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;

		public EmployeeController(IEmployeeService employeeService) 
		{
			_employeeService = employeeService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllClients()
		{
			return await _employeeService.GetAllEmployees();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClientById(int id)
		{
			return await _employeeService.GetEmployeeById(id);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateClient([FromBody] EmployeeDto employee)
		{
			return await _employeeService.CreateEmployee(employee);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateClient([FromBody] EmployeeDto employee)
		{
			return await _employeeService.UpdateEmployee(employee);
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteClient(int id)
		{
			return await _employeeService.DeleteEmployee(id);
		}
	}
}
