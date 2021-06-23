using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aspose.Cells;
using DKS_API.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiActionFilter))]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected ILogger<ApiController> _logger;

        //constructor

        public ApiController(IConfiguration config, IWebHostEnvironment webHostEnvironment,ILogger<ApiController> logger)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
       
    }
}