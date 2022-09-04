namespace Common.Results
{
    public abstract class BaseResult
    {
        public string[] Messages { get; set; }
        public string ErrorCode { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorCode);
    }
    
    public abstract class BaseResult<T> : BaseResult
    {
        public T Data { get; set; }
    }
}