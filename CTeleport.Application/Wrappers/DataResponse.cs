namespace CTeleport.Application.Wrappers
{
    public class DataResponse<T> : Response
    {
        public T Data { get; set; }

        public static DataResponse<T> Success(T data) => new DataResponse<T> { IsSuccess = true, Data = data };
    }
}