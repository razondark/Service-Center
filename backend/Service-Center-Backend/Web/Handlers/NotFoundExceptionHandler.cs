namespace Service_Center_Backend.Web.Handlers
{
	public class NotFoundExceptionHandler : BaseException
	{
		public NotFoundExceptionHandler(string message) : base(message) { }
	}
}
