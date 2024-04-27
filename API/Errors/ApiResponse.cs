namespace ShopeStore.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Unauthorized: Access is denied due to invalid credentials.",
                404 => "The requested resource was not found.",
                500 => "Internal server error: Something went wrong on the server side.",
                _ => "Unknown status code."
            };
        }
    }
}
