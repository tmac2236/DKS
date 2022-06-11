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

namespace DKS_API.Controllers
{
    public class WareHouseController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly IWarehouseDAO _warehouseDao;
        private readonly ISamPartBDAO _samPartBDAO;
        public WareHouseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger, IWarehouseDAO warehouseDao, ISamPartBDAO samPartBDAO,
        IExcelService excelService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _warehouseDao = warehouseDao;
            _samPartBDAO = samPartBDAO;
        }
        [HttpGet("getMaterialNoBySampleNoForWarehouse")]
        public IActionResult GetMaterialNoBySampleNoForWarehouse([FromQuery] SF428SampleNoDetail sF428SampleNoDetail)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController GetMaterialNoBySampleNoForWarehouse fired!! ******"));

            //sF428SampleNoDetail.SampleNo ="FW21-SMS-GZ7884-01";

            var result = _warehouseDao.GetMaterialNoBySampleNoForWarehouse(sF428SampleNoDetail);

            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }

        [HttpPost("exportMaterialNoBySampleNoForWarehouse")]
        public IActionResult ExportMaterialNoBySampleNoForWarehouse(SF428SampleNoDetail sF428SampleNoDetail)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController ExportMaterialNoBySampleNoForWarehouse fired!! ******"));

            // query data from database  
            var data = _warehouseDao.GetMaterialNoBySampleNoForWarehouse(sF428SampleNoDetail);

            byte[] result = _excelService.CommonExportReport(data, "TempF428.xlsx");

            return File(result, "application/xlsx");
        }
        [HttpPost("getStockDetailByMaterialNo")]
        public async Task<IActionResult> GetStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController GetStockDetailByMaterialNo fired!! ******"));

            var data = await _warehouseDao.GetStockDetailByMaterialNo(sF428SampleNoDetail);
            return Ok(data);

        }
        [HttpPost("addStockDetailByMaterialNo")]
        public async Task<IActionResult> AddStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController AddStockDetailByMaterialNo fired!! ******"));

            SamPartB model = await _samPartBDAO.FindAll(x => x.SAMPLENO.Trim() == sF428SampleNoDetail.SampleNo.Trim() &&
                                                 x.MATERIANO == sF428SampleNoDetail.MaterialNo.Trim()).FirstOrDefaultAsync();
            if (model != null)
            {
                model.STATUS = sF428SampleNoDetail.Status;
                model.CHKSTOCKNO = sF428SampleNoDetail.ChkStockNo;
                model.CHKUSR = sF428SampleNoDetail.loginUser;
                model.CHKTIME = DateTime.Now;
                _samPartBDAO.Update(model);
            }
            await _samPartBDAO.SaveAll();
            return Ok();

        }
        [HttpGet("getF406iDto")]
        public async Task<IActionResult> GetF406iDto([FromQuery] SF406i sF406i)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController GetF406iDto fired!! ******"));

            var data = await _warehouseDao.GetF406iDto(sF406i);

            PagedList<F406iDto> result = PagedList<F406iDto>.Create(data, sF406i.PageNumber, sF406i.PageSize, sF406i.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }
        [HttpGet("getP406Dto")]
        public async Task<IActionResult> GetP406Dto([FromQuery] string acpDateS, string acpDateE)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController GetF406iDto fired!! ******"));

            var data = await _warehouseDao.GetP406Dto(acpDateS, acpDateE);

            byte[] result = _excelService.CommonExportReport(data, "TempP406.xlsx");
            return File(result, "application/xlsx");
        }
        [HttpGet("getF434Dto")]
        public async Task<IActionResult> GetF434Dto([FromQuery] SF406i sF406i)
        {
            _logger.LogInformation(String.Format(@"****** WareHouseController GetF434Dto fired!! ******"));

            var data = await _warehouseDao.GetF434Dto(sF406i);

            PagedList<F434Dto> result = PagedList<F434Dto>.Create(data, sF406i.PageNumber, sF406i.PageSize, sF406i.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }        
    }
}