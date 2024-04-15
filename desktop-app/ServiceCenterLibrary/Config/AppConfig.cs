using System.Configuration;

namespace ServiceCenterLibrary.Config
{
	public class AppConfig
	{
		// clients links
		public readonly string GetAllClientsLink;
		public readonly string GetClientByIdLink;
		public readonly string CreateClientLink;
		public readonly string UpdateClientLink;
		public readonly string DeleteClientLink;

		// employees links
		public readonly string GetAllEmployeesLink;
		public readonly string GetEmployeeByIdLink;
		public readonly string CreateEmployeeLink;
		public readonly string UpdateEmployeeLink;
		public readonly string DeleteEmployeeLink;

		// devices links
		public readonly string GetAllDevicesLink;
		public readonly string GetDeviceByIdLink;
		public readonly string CreateDeviceLink;
		public readonly string UpdateDeviceLink;
		public readonly string DeleteDeviceLink;

		// ehr links
		public readonly string GetAllEHRLink;
		public readonly string GetEHRByIdLink;
		public readonly string CreateEHRLink;
		public readonly string UpdateEHRLink;
		public readonly string DeleteEHRLink;

		public AppConfig()
		{
			// clients
			GetAllClientsLink = ConfigurationManager.AppSettings["GetAllClients"]!;
			GetClientByIdLink = ConfigurationManager.AppSettings["GetClientById"]!;
			CreateClientLink = ConfigurationManager.AppSettings["CreateClient"]!;
			UpdateClientLink = ConfigurationManager.AppSettings["UpdateClient"]!;
			DeleteClientLink = ConfigurationManager.AppSettings["DeleteClient"]!;

			// employees
			GetAllEmployeesLink = ConfigurationManager.AppSettings["GetAllEmployees"]!;
			GetEmployeeByIdLink = ConfigurationManager.AppSettings["GetEmployeeById"]!;
			CreateEmployeeLink = ConfigurationManager.AppSettings["CreateEmployee"]!;
			UpdateEmployeeLink = ConfigurationManager.AppSettings["UpdateEmployee"]!;
			DeleteEmployeeLink = ConfigurationManager.AppSettings["DeleteEmployee"]!;

			// devices
			GetAllDevicesLink = ConfigurationManager.AppSettings["GetAllDevices"]!;
			GetDeviceByIdLink = ConfigurationManager.AppSettings["GetDeviceById"]!;
			CreateDeviceLink = ConfigurationManager.AppSettings["CreateDevice"]!;
			UpdateDeviceLink = ConfigurationManager.AppSettings["UpdateDevice"]!;
			DeleteDeviceLink = ConfigurationManager.AppSettings["DeleteDevice"]!;

			// ehr
			GetAllEHRLink = ConfigurationManager.AppSettings["GetAllEHR"]!;
			GetEHRByIdLink = ConfigurationManager.AppSettings["GetEHRById"]!;
			CreateEHRLink = ConfigurationManager.AppSettings["CreateEHR"]!;
			UpdateEHRLink = ConfigurationManager.AppSettings["UpdateEHR"]!;
			DeleteEHRLink = ConfigurationManager.AppSettings["DeleteEHR"]!;
		}
	}
}
