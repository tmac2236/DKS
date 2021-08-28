using DFPS.API.Data.Repository;
using DKS.API.Models.DKS;
using DKS_API.Controllers;
using DKS_API.Data;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFPS.API.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        private readonly IArticledLdtmDAO _dao;
        private readonly IArticledDAO _articledDAO;
        public TestController(IConfiguration config, IWebHostEnvironment webHostEnvironment,ILogger<TestController> logger,
                IArticledLdtmDAO dao, IArticledDAO articledDAO)
        : base(config, webHostEnvironment,logger)
        {
            _dao = dao;
            _articledDAO = articledDAO;
        }
        
        [AllowAnonymous]
        [HttpGet("testGetFile")]
        public IActionResult testGetFile()
        {
            _logger.LogInformation(String.Format(@"****** TestController testGetFile fired!! ******"));

            ArticledLdtm model = _dao.FindSingle( x =>x.PKARTBID == "CD0000020155");
            byte[] result = model.ATTACHED_DATA;

            return File(result, "",model.ATTACHED_DATA_NAME);
        }
        [AllowAnonymous]
        [HttpGet("getSeasonNum")]
        public async Task<IActionResult>  GetSeasonNum()
        {
            _logger.LogInformation(String.Format(@"****** TestController GetSeasonNum fired!! ******"));

            var data =  await _articledDAO.GetSeasonNumDto();
            return Ok(data);
        }

    }
}