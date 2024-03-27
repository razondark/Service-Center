namespace Service_Center_Backend.Web.Handlers
{
	public class BaseException
	{
		public string Message { get; private set; } = "Server error";

		public BaseException() { }

		public BaseException(string message) 
		{
			Message = message;
		}
	}
}
