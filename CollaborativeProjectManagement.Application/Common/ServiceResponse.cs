namespace CollaborativeProjectManagement.Application.Common
{
    public class ServiceResponse
    {
        public required bool Success { get; set; }
        public required int StatusCode { get; set; }
        public string? Message { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }
    }
}
