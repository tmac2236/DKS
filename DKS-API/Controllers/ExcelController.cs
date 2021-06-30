using DKS_API.Controllers;
using DKS_API.Data.Repository;
using DKS_API.DTOs;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFPS.API.Controllers
{
    //Only execute stored procedure not do DAO layer but using context directly
    public class ExcelController : ApiController
    {
        private readonly DKSContext _context;
        private readonly IExcelService _excelService;
        public ExcelController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<TestController> logger, DKSContext context, IExcelService excelService)
        : base(config, webHostEnvironment, logger)
        {
            _context = context;
            _excelService = excelService;
        }

        [HttpPost("getP206DataByArticle")]
        public async Task<IActionResult> GetP206DataByArticle(SExcelHome sExcelHome)
        {
            _logger.LogInformation(String.Format(@"****** ExcelController GetP206DataByArticle fired!! ******"));

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Article",sExcelHome.article)
            };
            var data = await _context.GetP206DataByArticle
                   .FromSqlRaw(" EXECUTE dbo.GetP206DataByArticle @Article ", pc.ToArray())
                   .ToListAsync();
            string title = "";
            if (data.Count > 0){
                 title = data[0].Title;
            }else{
                return Ok("0");
            }

            byte[] result = _excelService.CommonExportReportWithATitle(data, "TempP206DataByArticle.xlsx", title);
            return File(result, "application/xlsx", "P206DataByArticle.xlsx");
        }


    }
}