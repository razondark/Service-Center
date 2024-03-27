namespace Service_Center_Backend.Web.Dto.Handlers
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
