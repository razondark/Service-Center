using System.Configuration;

namespace ServiceCenterLibrary.Config
{
	public class AppConfig
	{
		// clients links
		public readonly string GetAllClientsLink;
		public readonly string CreateClientLink;
		public readonly string UpdateClientLink;
		public readonly string DeleteClientLink;

		// employees links
		public readonly string GetAllEmployeesLink;
		public readonly string CreateEmployeeLink;
		public readonly string UpdateEmployeeLink;
		public readonly string DeleteEmployeeLink;

		public AppConfig()
		{
			// clients
			GetAllClientsLink = ConfigurationManager.AppSettings["GetAllClients"]!;
			CreateClientLink = ConfigurationManager.AppSettings["CreateClient"]!;
			UpdateClientLink = ConfigurationManager.AppSettings["UpdateClient"]!;
			DeleteClientLink = ConfigurationManager.AppSettings["DeleteClient"]!;

			// employees
			GetAllEmployeesLink = ConfigurationManager.AppSettings["GetAllEmployees"]!;
			CreateEmployeeLink = ConfigurationManager.AppSettings["CreateEmployee"]!;
			UpdateEmployeeLink = ConfigurationManager.AppSettings["UpdateEmployee"]!;
			DeleteEmployeeLink = ConfigurationManager.AppSettings["DeleteEmployee"]!;
		}
	}
}
