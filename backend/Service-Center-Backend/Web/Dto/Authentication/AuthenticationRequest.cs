namespace Service_Center_Backend.Web.Dto.Authentication
{
	public class AuthenticationRequest
	{
		public string Login { get; private set; }
		public string Password { get; private set; }

		public AuthenticationRequest(string login, string password)
		{
			Login = login;
			Password = password;
		}
	}
}
