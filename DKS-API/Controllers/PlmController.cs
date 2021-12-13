using System;
using System.Threading.Tasks;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using DKS_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DKS.API.Models.DKS;
using Microsoft.AspNetCore.Hosting;
using DKS_API.Services.Implement;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DKS_API.Controllers
{
    public class PlmController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly IDevPlmPartDAO _devPlmPartDAO;
        public PlmController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger, IDevPlmPartDAO devPlmPartDAO,
             IExcelService excelService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _devPlmPartDAO = devPlmPartDAO;
        }
        [HttpGet("getPlmPart")]
        public  IActionResult GetPlmPart([FromQuery] SDevPlmPart sDevPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController GetPlmPart fired!! ******"));

            var data =  _devPlmPartDAO.FindAll();
            if(!String.IsNullOrEmpty(sDevPlmPart.partno)) data = data.Where(x => x.PARTNO == sDevPlmPart.partno.Trim());
            if(!String.IsNullOrEmpty(sDevPlmPart.location)) data = data.Where(x => x.LOCATION.Contains(sDevPlmPart.location.Trim()) );
            if(!String.IsNullOrEmpty(sDevPlmPart.partnameen)) data = data.Where(x => x.PARTNAMEEN.Contains(sDevPlmPart.partnameen.Trim()) );
            if(!String.IsNullOrEmpty(sDevPlmPart.partnamecn)) data = data.Where(x => x.PARTNAMECN.Contains(sDevPlmPart.partnamecn.Trim()) );
            data = data.OrderBy(x => x.PARTNO);

            PagedList<DevPlmPart> result = PagedList<DevPlmPart>.Create(data, sDevPlmPart.PageNumber, sDevPlmPart.PageSize, sDevPlmPart.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }
        [HttpGet("checkPartNoIsExist")]
        public  IActionResult CheckPartNoIsExist(string partno)
        {
            _logger.LogInformation(String.Format(@"****** PlmController CheckPartNoIsExist fired!! ******"));

            var data =  _devPlmPartDAO.FindSingle(x =>x.PARTNO == partno.Trim());
            var isExist = false;
            if( data != null) isExist= true;
            return Ok(isExist);

        }        
        [HttpPost("addPlmPart")]
        public async Task<IActionResult> AddPlmPart(DevPlmPart devPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController AddPlmPart fired!! ******"));

            devPlmPart.INSERTDATE = DateTime.Now;
            devPlmPart.CHANGEDATE = DateTime.Now;
            _devPlmPartDAO.Add(devPlmPart);
            await _devPlmPartDAO.SaveAll();

            return Ok(true);
        }        
        [HttpPost("updatePlmPart")]
        public async Task<IActionResult> UpdatePlmPart(DevPlmPart devPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController UpdatePlmPart fired!! ******"));

            devPlmPart.CHANGEDATE = DateTime.Now;
            _devPlmPartDAO.Update(devPlmPart);
            await _devPlmPartDAO.SaveAll();

            return Ok(true);

        } 
        [HttpPost("deletePlmPart")]
        public async Task<IActionResult> DeletePlmPart(DevPlmPart devPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController DeletePlmPart fired!! ******"));

            _devPlmPartDAO.Remove(devPlmPart);
            await _devPlmPartDAO.SaveAll();

            return Ok(true);
        }
        [HttpPost("exportPlmPart")]
        public  IActionResult ExportPlmPart(SDevPlmPart sDevPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController ExportPlmPart fired!! ******"));

            var data =  _devPlmPartDAO.FindAll();
            if(!String.IsNullOrEmpty(sDevPlmPart.partno)) data = data.Where(x => x.PARTNO == sDevPlmPart.partno.Trim());
            if(!String.IsNullOrEmpty(sDevPlmPart.location)) data = data.Where(x => x.LOCATION.Contains(sDevPlmPart.location.Trim()) );
            if(!String.IsNullOrEmpty(sDevPlmPart.partnameen)) data = data.Where(x => x.PARTNAMEEN.Contains(sDevPlmPart.partnameen.Trim()) );
            if(!String.IsNullOrEmpty(sDevPlmPart.partnamecn)) data = data.Where(x => x.PARTNAMECN.Contains(sDevPlmPart.partnamecn.Trim()) );
            data = data.OrderBy(x => x.PARTNO);

            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempPlmPart.xlsx");

            return File(result, "application/xlsx");
        }                 
    }
}