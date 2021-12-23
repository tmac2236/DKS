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
        private readonly IDKSDAO _dksDao;
        public PlmController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger,
             IDevPlmPartDAO devPlmPartDAO, IDKSDAO dksDao,
             IExcelService excelService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _devPlmPartDAO = devPlmPartDAO;
            _dksDao = dksDao;
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
            
            DevPlmPart old = _devPlmPartDAO.FindAll(x =>x.PARTNO == devPlmPart.PARTNO).AsNoTracking().First();

            devPlmPart.CHANGEDATE = DateTime.Now;
            _devPlmPartDAO.Update(devPlmPart);
            await _devPlmPartDAO.SaveAll();
            if(old != null){
                UserLog userlog = new UserLog();
                userlog.PROGNAME = "PLM Part";
                userlog.LOGINNAME = devPlmPart.CHANGEUSER;
                userlog.HISTORY = string.Format("U, PartNo: {0}, PartName(En): {1}, PartName(Cn): {2}, PartName(Vn):{3}, Location: {4}, Rename:{5}, PartGroup: {6}",
                                            old.PARTNO, old.PARTNAMEEN, old.PARTNAMECN, old.PARTNAMEVN, old.LOCATION, old.RENAME, old.PARTGROUP );
                userlog.UPDATETIME = DateTime.Now;
                await _dksDao.AddUserLogAsync(userlog);
            }

            return Ok(true);

        } 
        [HttpPost("deletePlmPart")]
        public async Task<IActionResult> DeletePlmPart(DevPlmPart devPlmPart)
        {
            _logger.LogInformation(String.Format(@"****** PlmController DeletePlmPart fired!! ******"));
                        UserLog userlog = new UserLog();
                        userlog.PROGNAME = "PLM Part";
                        userlog.LOGINNAME = devPlmPart.CHANGEUSER;
                        userlog.HISTORY = string.Format("D,PartNo: {0}", devPlmPart.PARTNO);
                        userlog.UPDATETIME = DateTime.Now;
                        await _dksDao.AddUserLogAsync(userlog);
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