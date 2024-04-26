using Skinet.API.Errors;

namespace ShopeStore.API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400) // just sent 400 status code (BadRequest)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
