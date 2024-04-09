using Microsoft.AspNetCore.Mvc;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Services
{
	public interface IEmployeeService
	{
		public Task<IActionResult> GetAllEmployees();

		public Task<IActionResult> GetEmployeeById(int id);

		public Task<IActionResult> CreateEmployee(EmployeeDto employeeDto);

		public Task<IActionResult> UpdateEmployee(EmployeeDto employeeDto);

		public Task<IActionResult> DeleteEmployee(int id);
	}
}
