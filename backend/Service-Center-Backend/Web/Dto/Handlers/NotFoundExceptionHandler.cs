namespace Service_Center_Backend.Web.Dto.Handlers
{
    public class NotFoundExceptionHandler : BaseException
    {
        public NotFoundExceptionHandler(string message) : base(message) { }
    }
}
