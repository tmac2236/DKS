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
using Aspose.Cells;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DKS_API.Controllers
{
    public class DKSController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly IDKSDAO _dksDao;
        private readonly IDevBuyPlanDAO _devBuyPlanDAO;
        public DKSController(IConfiguration config, IDKSDAO dksDao,IDevBuyPlanDAO devBuyPlanDAO)

        {
            _dksDao = dksDao;
            _config = config;
            _devBuyPlanDAO = devBuyPlanDAO;
        }
        [HttpGet("exportF340_Process")]
        public async Task<IActionResult> ExportF340_Process(string season,string bpVer)
        {
            // query data from database  
            var data = await _dksDao.GetF340ProcessView(season, bpVer);

            WorkbookDesigner designer = new WorkbookDesigner();
            #region style設計
            //Create style object(標題用)
            Style titleStyle = designer.Workbook.CreateStyle();
            titleStyle.Pattern = BackgroundType.Solid;
            titleStyle.ForegroundColor = Color.LightGreen;
            //Create style flag(標題用)
            StyleFlag titleFlag = new StyleFlag();
            titleFlag.CellShading = true;
            #endregion style設計

            Worksheet ws = designer.Workbook.Worksheets[0];
            //titile
            ws.Cells[0, 0].Value = "buyPlanSeason";
            ws.Cells[0, 1].Value = "versionNo";
            ws.Cells[0, 2].Value = "devSeason";
            ws.Cells[0, 3].Value = "article";
            ws.Cells[0, 4].Value = "cwaDeadline";

            ws.Cells[0, 5].Value = "modelNo";
            ws.Cells[0, 6].Value = "modelName";
            ws.Cells[0, 7].Value = "bomStage";
            ws.Cells[0, 8].Value = "sampleNo";
            ws.Cells[0, 9].Value = "devStatus";

            ws.Cells[0, 10].Value = "dropDate";
            ws.Cells[0, 11].Value = "hpFlag";
            ws.Cells[0, 12].Value = "hpSampleNo";
            ws.Cells[0, 13].Value = "f340SampleNo";
            ws.Cells[0, 14].Value = "releaseType";

            ws.Cells[0, 15].Value = "createDate";
            ws.Cells[0, 16].Value = "pdmDate";
            ws.Cells[0, 17].Value = "devUpDate";
            ws.Cells[0, 18].Value = "devBtmDate";
            ws.Cells[0, 19].Value = "ttUpDate";

            ws.Cells[0, 20].Value = "ttBtmDate";
            ws.Cells[0, 21].Value = "ttRejectReason";
            ws.Cells[0, 22].Value = "ttRejectDate";

            ws.Cells.Rows[0].ApplyStyle(titleStyle, titleFlag);//第一列套上顏色
            ws.FreezePanes(1, 1, 1, 1);
            int row = 1;
            foreach (F340_ProcessDto item in data)
            {
                ws.Cells[row, 0].Value = item.BuyPlanSeason;
                ws.Cells[row, 1].Value = item.VersionNo;
                ws.Cells[row, 2].Value = item.DevSeason;
                ws.Cells[row, 3].Value = item.Article;
                ws.Cells[row, 4].Value = item.CwaDeadline;

                ws.Cells[row, 5].Value = item.ModelNo;
                ws.Cells[row, 6].Value = item.ModelName;
                ws.Cells[row, 7].Value = item.BomStage;
                ws.Cells[row, 8].Value = item.SampleNo;
                ws.Cells[row, 9].Value = item.DevStatus;

                ws.Cells[row, 10].Value = item.DropDate;
                ws.Cells[row, 11].Value = item.HpFlag;
                ws.Cells[row, 12].Value = item.HpSampleNo;
                ws.Cells[row, 13].Value = item.F340SampleNo;
                ws.Cells[row, 14].Value = item.ReleaseType;

                ws.Cells[row, 15].Value = item.CreateDate;
                ws.Cells[row, 16].Value = item.PdmDate;
                ws.Cells[row, 17].Value = item.DevUpDate;
                ws.Cells[row, 18].Value = item.DevBtmDate;
                ws.Cells[row, 19].Value = item.TTUpDate;

                ws.Cells[row, 20].Value = item.TTBtmDate;
                ws.Cells[row, 21].Value = item.TTRejectReason;
                ws.Cells[row, 22].Value = item.TTRejectDate; 
                row += 1;
            }
            ws.AutoFitColumns();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        [HttpGet("getF340_Process")]
        public async Task<IActionResult> GetF340_Process(string season, string bpVer)
        {
            try
            {

                var result = await _dksDao.GetF340ProcessView(season,bpVer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}.");
            }
        }
        [HttpGet("getBPVersionBySeason")]
        public async Task<IActionResult> GetBPVersionBySeason(string season)
        {
            try
            {

                var result = await _devBuyPlanDAO.FindAll(x => x.SEASON.Trim() == season.ToUpper().Trim())
                                        .Select(x => new
                                        {
                                            VERN = x.VERN
                                        }).Distinct()
                                        .ToListAsync();
                List<string> bpVern = new List<string>();
                foreach(var r in result){
                    bpVern.Add(r.VERN.ToString());
                }
                
                return Ok(bpVern);
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