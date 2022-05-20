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
        private readonly IDKSDAO _dksDAO;

        public SystemController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<SystemController> logger,
                 IDevSysSetDAO devSysSetDAO,IDKSDAO dksDAO)
                : base(config, webHostEnvironment, logger)
        {
            _devSysSetDAO = devSysSetDAO;
            _dksDAO = dksDAO;
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
        [HttpGet("getKanbanDataByLineDto")]
        public async Task<IActionResult> GetKanbanDataByLineDto(string lineId)
        {
            _logger.LogInformation(String.Format(@"****** SystemController GetKanbanDataByLineDto fired!! ******"));
            var data = await _dksDAO.GetKanbanDataByLineDto(lineId);
            return Ok(data);
        }           
        [HttpGet("getKanbanTQCDto")]
        public async Task<IActionResult> GetKanbanTQCDto(string lineId)
        {
            _logger.LogInformation(String.Format(@"****** SystemController GetKanbanTQCDto fired!! ******"));
            var data = await _dksDAO.GetKanbanTQCDto(lineId);
            return Ok(data);
        }        

    }
}