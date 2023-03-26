using CTeleport.Domain.Enums;

namespace CTeleport.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public ErrorType Type { get; set; }

        public BusinessException(ErrorType type, string errorMessage) : base(errorMessage)
        {
            this.Type = type;
        }
    }
}