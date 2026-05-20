using System.Net;

namespace CollaborativeProjectManagement.Application.Common
{
    public class ServiceResponse
    {
        public required bool Success { get; set; }
        public required HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public static ServiceResponse Ok(string? message) => new() { Success = true, StatusCode = HttpStatusCode.OK, Message = message };
        public static ServiceResponse NotFound(string? message) => new() { Success = false, StatusCode = HttpStatusCode.NotFound, Message = message };
        public static ServiceResponse Forbidden(string? message) => new() { Success = false, StatusCode = HttpStatusCode.Forbidden, Message = message };
        public static ServiceResponse Conflict(string? message) => new() { Success = false, StatusCode = HttpStatusCode.Conflict, Message = message };
        public static ServiceResponse InternalServerError(string? message) => new() { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = message };
        public static ServiceResponse NoContent(string? message) => new() { Success = false, StatusCode = HttpStatusCode.NoContent, Message = message };
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T? data, string? message) => new() { Success = true, StatusCode = HttpStatusCode.OK, Data = data, Message = message };
        public static ServiceResponse<T> Created(T? data, string? message) => new() { Success = true, StatusCode = HttpStatusCode.Created, Data = data, Message = message };
        public static ServiceResponse<T> NotFound(T? data, string? message) => new() { Success = false, StatusCode = HttpStatusCode.NotFound, Data = data, Message = message };
        public static ServiceResponse<T> Unauthorized(T? data, string? message) => new() { Success = false, StatusCode = HttpStatusCode.Unauthorized, Data = data, Message = message };
        public static ServiceResponse<T> Forbidden(T? data, string? message) => new() { Success = false, StatusCode = HttpStatusCode.Forbidden, Data = data, Message = message };
    }
}
