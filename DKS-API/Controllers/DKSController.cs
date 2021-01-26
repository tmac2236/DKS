using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Drawing;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Http;
using Aspose.Cells;
using System.Data;

namespace DKS_API.Controllers
{
    public class DKSController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly IDKSDAO _dksDao;

        public DKSController(IConfiguration config, IDKSDAO dksDao)

        {
            _dksDao = dksDao;
            _config = config;
        }
        [HttpGet("getF340_Process")]
        public async Task<IActionResult> GetF340_Process(string startDate,string endDate)
        {
            try
            {
                var result = await _dksDao.GetF340ProcessView(startDate,endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}.");
            }
        }
        [HttpPost("checkF420Valid")]
        public IActionResult CheckF420Valid([FromForm] ArticlePic excel)
        {
            int processIndex = 0;//use in debug
            try
            {
                string rootdir = Directory.GetCurrentDirectory();
                string filePath = rootdir + "\\Resources\\Temp";
                var fileName = "F420.xls";
                //新增檔名的全路徑
                var fullPath = Path.Combine(filePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    excel.File.CopyTo(stream);
                }

                List<F418_F420Dto> list = new List<F418_F420Dto>();
                //read excel
                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(fullPath);
                Worksheet ws = wk.Worksheets[0];
                DataTable dt = ws.Cells.ExportDataTable(0, 0, ws.Cells.MaxDataRow + 1, ws.Cells.MaxDataColumn + 1);

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    processIndex = i;
                    string orderNo = dt.Rows[i][1].ToString().Trim();  //Order No
                    string qty = dt.Rows[i][30].ToString().Trim();     //廠商出貨數量
                    if (orderNo == "" || qty == "") continue; //如果任一沒輸入忽略
                    decimal nQty = decimal.Parse(qty);
                    //檢查該訂購單號還有幾個要驗收
                    var result = _dksDao.GetF420F418View(orderNo);
                    decimal compare = nQty - result.NEEDQTY;
                    if (compare > 0)    //驗收數量大於數量
                    {
                        result.NEEDQTY = compare;
                        list.Add(result);
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}. DKS_DEBUG: The row is {processIndex}");
            }
        }
    }
}