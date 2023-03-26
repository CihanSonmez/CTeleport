using CTeleport.Domain.Enums;

namespace CTeleport.Application.Wrappers
{
    public class ErrorModel
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
    }
}