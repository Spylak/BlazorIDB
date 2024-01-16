namespace BlazorIDB
{
    public class ResponseIDB
    {
        public ResponseIDB(bool isSuccess, string? message = null, ErrorCode? errorCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
        }
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public ErrorCode? ErrorCode { get; init; }
    }
    public class ResponseIDB<T> : ResponseIDB where T : class?
    {
        public ResponseIDB(T? data, bool isSuccess, string? message = null, ErrorCode? errorCode = null) : base(isSuccess, message, errorCode)
        {
            Data = data;
        }
        public T? Data { get; set; }
    }
}
