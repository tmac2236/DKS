using Microsoft.AspNetCore.Mvc;
using System;
using DFPS_API.Filters;

namespace DFPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController : ControllerBase
    {

    }
}