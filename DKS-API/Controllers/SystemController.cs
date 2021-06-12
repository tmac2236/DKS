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

namespace DKS_API.Controllers
{
    public class SystemController : ApiController
    {
        private readonly IDevSysSetDAO _devSysSetDAO;

        public SystemController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IDevSysSetDAO devSysSetDAO)
                : base(config, webHostEnvironment)
        {
            _devSysSetDAO = devSysSetDAO;
        }

        [HttpGet("findAll")]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var result = await _devSysSetDAO.GetAll().ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost("eidtSysSet")]
        public async Task<IActionResult> EidtSysSet(DevSysSet devSysSet)
        {
            string errorStr = "";
            try
            {
                DevSysSet opRecord = _devSysSetDAO.FindSingle(
                x => x.SYSKEY.Trim() == devSysSet.SYSKEY.Trim());


                opRecord.SYSVAL = devSysSet.SYSVAL.Trim();
                opRecord.UPUSR = devSysSet.UPUSR.Trim();
                opRecord.UPTIME = DateTime.Now;
                _devSysSetDAO.Update(opRecord);
                await _devSysSetDAO.SaveAll();
                return Ok(errorStr);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}