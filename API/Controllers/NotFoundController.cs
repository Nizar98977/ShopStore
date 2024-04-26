using Microsoft.AspNetCore.Mvc;
using Skinet.API.Errors;

namespace Skinet.API.Controllers
{
    public class NotFoundController : Controller
    {
        [Route("[controller]/{code}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            //This endpoint will be hit when the clint hit on an endpoint that is not exist
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
