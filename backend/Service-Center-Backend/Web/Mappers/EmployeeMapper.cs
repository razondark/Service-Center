using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Mappers
{
	public static class EmployeeMapper
	{
		public static EmployeeDto ToDto(Employee employee)
		{
			return new EmployeeDto()
			{
				Id = employee.Id,
				FullName = employee.FullName,
				Passport = employee.Passport,
				PhoneNumber = employee.PhoneNumber,
				Email = employee.Email,
				Position = employee.Position,
			};
		}

		public static IList<EmployeeDto> ToDto(IList<Employee> employees)
		{
			var listEmployeeDto = new List<EmployeeDto>(employees.Count);

			foreach (var employee in employees)
			{
				listEmployeeDto.Add(ToDto(employee));
			}

			return listEmployeeDto;
		}

		public static Employee ToModel(EmployeeDto employee)
		{
			return new Employee()
			{
				Id = employee.Id,
				FullName = employee.FullName,
				Passport = employee.Passport,
				PhoneNumber = employee.PhoneNumber,
				Email = employee.Email,
				Position = employee.Position,
			};
		}
	}
}
