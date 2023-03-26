using CTeleport.Domain.Enums;

namespace CTeleport.Application.Wrappers
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public ErrorModel Error { get; set; }

        public static Response Success() => new Response { IsSuccess = true };
        public static Response Fail(ErrorType errorType, string errorMessage) => new Response
        {
            IsSuccess = false,
            Error = new ErrorModel
            {
                Type = errorType,
                Message = errorMessage
            }
        };
    }
}