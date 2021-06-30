using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Drawing;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DKS_API.Controllers
{
    public class SystemController : ApiController
    {
        private readonly IDevSysSetDAO _devSysSetDAO;

        public SystemController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<SystemController> logger, IDevSysSetDAO devSysSetDAO)
                : base(config, webHostEnvironment, logger)
        {
            _devSysSetDAO = devSysSetDAO;
        }

        [HttpGet("findAll")]
        public async Task<IActionResult> FindAll()
        {
            _logger.LogInformation(String.Format(@"****** SystemController FindAll fired!! ******"));

            var result = await _devSysSetDAO.GetAll().ToListAsync();
            return Ok(result);

        }
        [HttpPost("eidtSysSet")]
        public async Task<IActionResult> EidtSysSet(DevSysSet devSysSet)
        {
            _logger.LogInformation(String.Format(@"****** SystemController EidtSysSet fired!! ******"));

            string errorStr = "";

            DevSysSet opRecord = _devSysSetDAO.FindSingle(
            x => x.SYSKEY.Trim() == devSysSet.SYSKEY.Trim());


            opRecord.SYSVAL = devSysSet.SYSVAL.Trim();
            opRecord.UPUSR = devSysSet.UPUSR.Trim();
            opRecord.UPTIME = DateTime.Now;
            _devSysSetDAO.Update(opRecord);
            await _devSysSetDAO.SaveAll();
            return Ok(errorStr);

        }

    }
}