using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DKS_API.Data.Interface;

namespace DKS_API.Controllers
{
    public class DksController : ApiController
    {
        private readonly IDKSDAO _iDKSDAO;
        private readonly ISPFactoryDAO _sPFactoryDAO;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DksController> _logger;
        public DksController(IDKSDAO iDKSDAO, IWebHostEnvironment webHostEnvironment, ILogger<DksController> logger)
        {
            _iDKSDAO = iDKSDAO;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        [HttpGet("throw")]
        public object Throw()
        {
            throw new Exception();
        }

        [HttpGet("searchConvergence")]
        public async Task<IActionResult> searchConvergence
        (string season, string stage)
        {
            // query data from database  
            var data = await _iDKSDAO.SearchConvergence(season, stage);
            var result = new List<string>();
            if (data.Count > 0)
            {
                result.Add(data[0].PRSUMNO); //first
                if (data.Count > 1)
                {
                    result.Add(data[data.Count - 1].PRSUMNO); //end if exist
                }
            }
            return Ok(result);
        }

    }
}