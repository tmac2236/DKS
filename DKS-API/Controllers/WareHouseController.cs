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

namespace DKS_API.Controllers
{
    public class WareHouseController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly IWarehouseDAO _warehouseDao;
        private readonly ISamPartBDAO _samPartBDAO;
        public WareHouseController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IWarehouseDAO warehouseDao, ISamPartBDAO samPartBDAO,
        IExcelService excelService)
        : base(config, webHostEnvironment)
        {
            _excelService = excelService;
            _warehouseDao = warehouseDao;
            _samPartBDAO = samPartBDAO;
        }
        [HttpGet("getMaterialNoBySampleNoForWarehouse")]
        public IActionResult GetMaterialNoBySampleNoForWarehouse([FromQuery] SF428SampleNoDetail sF428SampleNoDetail)
        {
            try
            {
                //sF428SampleNoDetail.SampleNo ="FW21-SMS-GZ7884-01";

                var result = _warehouseDao.GetMaterialNoBySampleNoForWarehouse(sF428SampleNoDetail);

                Response.AddPagination(result.CurrentPage, result.PageSize,
                result.TotalCount, result.TotalPages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}.");
            }
        }

        [HttpPost("exportMaterialNoBySampleNoForWarehouse")]
        public IActionResult ExportMaterialNoBySampleNoForWarehouse(SF428SampleNoDetail sF428SampleNoDetail)
        {

            // query data from database  
            var data = _warehouseDao.GetMaterialNoBySampleNoForWarehouse(sF428SampleNoDetail);

            byte[] result = _excelService.CommonExportReport(data, "TempF428.xlsx");

            return File(result, "application/xlsx");
        }
        [HttpPost("getStockDetailByMaterialNo")]
        public async Task<IActionResult> GetStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
            var data = await _warehouseDao.GetStockDetailByMaterialNo(sF428SampleNoDetail);
            return Ok(data);

        }
        [HttpPost("addStockDetailByMaterialNo")]
        public async Task<IActionResult> AddStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
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
    }
}