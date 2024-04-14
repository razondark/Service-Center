using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Center_Backend.Context;
using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto.Handlers;
using Service_Center_Backend.Web.Dto;
using Service_Center_Backend.Web.Mappers;

namespace Service_Center_Backend.Services.Implements
{
	public class EmployeeServiceImplement : IEmployeeService
	{
		private readonly ServiceCenterContext _context;

		public EmployeeServiceImplement(ServiceCenterContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto)
		{
			try
			{
				employeeDto.Id = default(int);
				_context.Employees.Add(EmployeeMapper.ToModel(employeeDto));
				await _context.SaveChangesAsync();

				return new ObjectResult(employeeDto) { StatusCode = StatusCodes.Status201Created };
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

		public async Task<IActionResult> DeleteEmployee(int id)
		{
			try
			{
				var employee = await _context.Employees.Where(c => c.Id == id).FirstOrDefaultAsync();
				if (employee is null)
				{
					return new NotFoundObjectResult(new NotFoundExceptionHandler("Сотрудник не найден"));
				}

				_context.Employees.Remove(employee);
				await _context.SaveChangesAsync();

				return new OkObjectResult(employee);
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

		public async Task<IActionResult> GetAllEmployees()
		{
			var employees = await _context.Employees.ToListAsync();
			if (employees is null || employees.Count == 0)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Сотрудники не найдены"));
			}

			return new OkObjectResult(EmployeeMapper.ToDto(employees));
		}

		public async Task<IActionResult> GetEmployeeById(int id)
		{
			var employee = await _context.Employees.Where(c => c.Id == id).FirstOrDefaultAsync();
			if (employee is null)
			{
				return new NotFoundObjectResult(new NotFoundExceptionHandler("Сотрудник не найден"));
			}

			return new OkObjectResult(EmployeeMapper.ToDto(employee));
		}

		public async Task<IActionResult> UpdateEmployee(EmployeeDto employeeDto)
		{
			try
			{
				_context.Employees.Update(EmployeeMapper.ToModel(employeeDto));
				await _context.SaveChangesAsync();

				return new OkObjectResult(employeeDto);
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
