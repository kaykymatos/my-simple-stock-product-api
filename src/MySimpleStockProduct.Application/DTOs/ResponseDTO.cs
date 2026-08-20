namespace MySimpleStockProduct.Application.DTOs
{
    public record ResponseDTO<T>
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
        public int StatusCode { get; init; }
        public IEnumerable<string>? Errors { get; init; }
        public Dictionary<string, string[]>? ValidationErrors { get; init; }

        public ResponseDTO<T> Ok(T? data, string? message = null, int statusCode = 200) =>
            new()
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = statusCode,
                Errors = Array.Empty<string>(),
                ValidationErrors = null
            };

        public ResponseDTO<T> Fail(IEnumerable<string>? errors = null, string? message = null, int statusCode = 400, Dictionary<string, string[]>? validationErrors = null) =>
            new()
            {
                Success = false,
                Data = default,
                Message = message,
                StatusCode = statusCode,
                Errors = errors ?? Array.Empty<string>(),
                ValidationErrors = validationErrors
            };

        public ResponseDTO<T> FromException(Exception exception, int statusCode = 500) =>
            new()
            {
                Success = false,
                Data = default,
                Message = exception.Message,
                StatusCode = statusCode,
                Errors = new[] { exception.ToString() },
                ValidationErrors = null
            };
    }
    public record ResponseDTO : ResponseDTO<object?>;
}
