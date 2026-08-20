namespace MySimpleStockProduct.Application.DTOs
{
    public record ResponseDTO<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }
        public Dictionary<string, string[]>? Errors { get; private set; }

        public void Ok(T? data, string? message = null, int statusCode = 200)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
            Errors = null;
        }

        public void Fail(string? message = null, int statusCode = 400, IDictionary<string, string[]>? validationErrors = null)
        {
            Success = false;
            Data = default;
            Message = message;
            StatusCode = statusCode;
            Errors = validationErrors?.ToDictionary();
        }

        public void FromException(Exception exception, int statusCode = 500)
        {
            Success = false;
            Data = default;
            Message = exception.Message;
            StatusCode = statusCode;
            Errors = new Dictionary<string, string[]>
            {
                { "InternalServerError", new[] { exception.ToString() } }
            };
        }
    }
    public record ResponseDTO : ResponseDTO<object?>;
}
