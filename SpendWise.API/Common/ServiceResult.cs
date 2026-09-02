namespace SpendWise.API.Common
{
    public class ServiceResult<T> 
    {
        public ServiceResultStatus Status { get; set; }

        public T? Data { get; set; }

        public string? Error { get; set; }
    }

    public enum ServiceResultStatus
    {
        Success,
        NotFound,
        ValidationError,
        Conflict
    }
}
