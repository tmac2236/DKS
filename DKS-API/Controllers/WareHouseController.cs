using System;
using System.Threading.Tasks;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using DKS_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Controllers
{
    public class WareHouseController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly IWarehouseDAO _warehouseDao;
        public WareHouseController(IConfiguration config, IWarehouseDAO warehouseDao)

        {
            _warehouseDao = warehouseDao;
            _config = config;
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
        [HttpPost("getStockDetailByMaterialNo")]
        public async Task<IActionResult> GetStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
            var data = await _warehouseDao.GetStockDetailByMaterialNo(sF428SampleNoDetail);
            return Ok(data);

        }
    }
}