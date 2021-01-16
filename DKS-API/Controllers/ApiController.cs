using DKS_API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController : ControllerBase
    {

    }
}