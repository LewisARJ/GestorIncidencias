namespace GestorIncidencias.Application.Core;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public int StatusCode { get; set; }

    public static ServiceResult<T> SuccessResponse(T? data, string message = "", int statusCode = 200)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ServiceResult<T> ErrorResponse(string message, int statusCode = 400)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode
        };
    }
}
