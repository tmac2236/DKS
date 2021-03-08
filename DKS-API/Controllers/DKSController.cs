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
using DKS_API.Helpers;

namespace DKS_API.Controllers
{
    public class DKSController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly IDKSDAO _dksDao;
        private readonly IDevBuyPlanDAO _devBuyPlanDAO;
        public DKSController(IConfiguration config, IDKSDAO dksDao, IDevBuyPlanDAO devBuyPlanDAO)

        {
            _dksDao = dksDao;
            _config = config;
            _devBuyPlanDAO = devBuyPlanDAO;
        }
        [HttpPost("exportF340_Process")]
        public async Task<IActionResult> ExportF340_Process(SF340Schedule sF340Schedule)
        {

            if (sF340Schedule.cwaDateS == "" || sF340Schedule.cwaDateS == null) sF340Schedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340Schedule.cwaDateE == "" || sF340Schedule.cwaDateE == null) sF340Schedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            // query data from database  
            var data = await _dksDao.GetF340ProcessView4Excel(sF340Schedule);

            WorkbookDesigner designer = new WorkbookDesigner();
            #region style設計
            //Create style object(標題用)
            Style hpStyle = designer.Workbook.CreateStyle();
            hpStyle.Pattern = BackgroundType.Solid;
            hpStyle.ForegroundColor = System.Drawing.ColorTranslator.FromHtml("#FF7F50");//背景顏色
            hpStyle.Font.Size = 12;                                 //字體大小
            hpStyle.HorizontalAlignment = TextAlignmentType.Center; //水平置中
            hpStyle.VerticalAlignment = TextAlignmentType.Center;   //垂直置中
            Style f204Style = designer.Workbook.CreateStyle();
            f204Style.Pattern = BackgroundType.Solid;
            f204Style.ForegroundColor = System.Drawing.ColorTranslator.FromHtml("#76DAFF");
            f204Style.Font.Size = 12;
            f204Style.HorizontalAlignment = TextAlignmentType.Center; //水平置中
            f204Style.VerticalAlignment = TextAlignmentType.Center;   //垂直置中
            Style f340Style = designer.Workbook.CreateStyle();
            f340Style.Pattern = BackgroundType.Solid;
            f340Style.ForegroundColor = System.Drawing.ColorTranslator.FromHtml("#23D954");
            f340Style.Font.Size = 12;
            f340Style.HorizontalAlignment = TextAlignmentType.Center; //水平置中
            f340Style.VerticalAlignment = TextAlignmentType.Center;   //垂直置中
            Style f240Style_Yellow = designer.Workbook.CreateStyle();
            f240Style_Yellow.Pattern = BackgroundType.Solid;
            f240Style_Yellow.ForegroundColor = System.Drawing.ColorTranslator.FromHtml("#EDD818");
            f240Style_Yellow.Font.Size = 12;
            f240Style_Yellow.HorizontalAlignment = TextAlignmentType.Center; //水平置中
            f240Style_Yellow.VerticalAlignment = TextAlignmentType.Center;   //垂直置中
            #endregion style設計

            Worksheet ws = designer.Workbook.Worksheets[0];
            //titile
            int row0 = 0;
            #region 第一列套上名稱及顏色 

            ws.Cells[row0, 0].Value = "HP系統";
            ws.Cells[row0, 4].Value = "開發系統F204/F205";
            ws.Cells[row0, 13].Value = "HP系統";
            ws.Cells[row0, 15].Value = "開發系統F340";
            ws.Cells[row0, 0].SetStyle(hpStyle);
            ws.Cells[row0, 4].SetStyle(f204Style);
            ws.Cells[row0, 13].SetStyle(hpStyle);
            ws.Cells[row0, 15].SetStyle(f340Style);
            ws.Cells.Merge(0, 0, 1, 3);
            ws.Cells.Merge(0, 3, 1, 10);
            ws.Cells.Merge(0, 13, 1, 2);
            ws.Cells.Merge(0, 15, 1, 12);
            #endregion 第一列套上名稱及顏色
            int row1 = 1;

            ws.Cells[row1, 0].Value = "廠別"; //factory
            ws.Cells[row1, 1].Value = "BUYPLAN季節";//"buyPlanSeason";
            ws.Cells[row1, 2].Value = "版本號";//"versionNo";
            ws.Cells[row1, 3].Value = "開發季節";//"devSeason";
            ws.Cells[row1, 4].Value = "DEV TEAM";//"devSeason";
            ws.Cells[row1, 5].Value = "ARTICLE";//"article";

            ws.Cells[row1, 6].Value = "CWADEADL";//"cwaDeadline";
            ws.Cells[row1, 7].Value = "MODELNO";//"modelNo";
            ws.Cells[row1, 8].Value = "MODELNAME";//"modelName";
            ws.Cells[row1, 9].Value = "ORDERSTAG";//"OrderStag";
            ws.Cells[row1, 10].Value = "最新樣品單號";//"sampleNo";

            ws.Cells[row1, 11].Value = "狀態碼";//"devStatus";
            ws.Cells[row1, 12].Value = "DROPDATE";//"dropDate";
            ws.Cells[row1, 13].Value = "HP_FLAG";//"hpFlag";
            ws.Cells[row1, 14].Value = "HP_SAMPLENO";//"hpSampleNo";
            ws.Cells[row1, 15].Value = "F340樣品單號";//"f340SampleNo";

            ws.Cells[row1, 16].Value = "資料型態";//"releaseType";
            ws.Cells[row1, 17].Value = "開單日期";//"createDate";
            ws.Cells[row1, 18].Value = "PDM維護日";//"pdmDate";
            ws.Cells[row1, 19].Value = "開發面部維護日";//"devUpDate";
            ws.Cells[row1, 20].Value = "開發底部維護日";//"devBtmDate";

            ws.Cells[row1, 21].Value = "技轉面部維護日";//"ttUpDate";
            ws.Cells[row1, 22].Value = "技轉底部維護日";//"ttBtmDate";
            ws.Cells[row1, 23].Value = "Rlease Date";//"ttBtmDate";
            ws.Cells[row1, 24].Value = "技轉退回原因";//"ttRejectReason";
            ws.Cells[row1, 25].Value = "技轉退回時間";//"ttRejectDate";

            ws.Cells[row1, 26].Value = "技轉退回次數";//"TTRejectCount";

            ws.Cells[row1, 0].SetStyle(hpStyle);
            ws.Cells[row1, 1].SetStyle(hpStyle);
            ws.Cells[row1, 2].SetStyle(hpStyle);
            ws.Cells[row1, 3].SetStyle(f204Style);
            ws.Cells[row1, 4].SetStyle(f204Style);
            ws.Cells[row1, 5].SetStyle(f204Style);
            ws.Cells[row1, 6].SetStyle(f204Style);
            ws.Cells[row1, 7].SetStyle(f204Style);
            ws.Cells[row1, 8].SetStyle(f204Style);
            ws.Cells[row1, 9].SetStyle(f204Style);
            ws.Cells[row1, 10].SetStyle(f204Style);
            ws.Cells[row1, 11].SetStyle(f240Style_Yellow);
            ws.Cells[row1, 12].SetStyle(f240Style_Yellow);
            ws.Cells[row1, 13].SetStyle(hpStyle);
            ws.Cells[row1, 14].SetStyle(hpStyle);
            ws.Cells[row1, 15].SetStyle(f340Style);
            ws.Cells[row1, 16].SetStyle(f340Style);
            ws.Cells[row1, 17].SetStyle(f340Style);
            ws.Cells[row1, 18].SetStyle(f340Style);
            ws.Cells[row1, 19].SetStyle(f340Style);
            ws.Cells[row1, 20].SetStyle(f340Style);
            ws.Cells[row1, 21].SetStyle(f340Style);
            ws.Cells[row1, 22].SetStyle(f340Style);
            ws.Cells[row1, 23].SetStyle(f340Style);
            ws.Cells[row1, 24].SetStyle(f340Style);
            ws.Cells[row1, 25].SetStyle(f340Style);
            ws.Cells[row1, 26].SetStyle(f340Style);
            #region 第二列套上名稱及顏色 

            #endregion 第二列套上名稱及顏色
            ws.FreezePanes(2, 2, 2, 2); //固定第一、二列

            int row = 2;
            foreach (F340_ProcessDto item in data)
            {
                ws.Cells[row, 0].Value = item.Factory;
                ws.Cells[row, 1].Value = item.BuyPlanSeason;
                ws.Cells[row, 2].Value = item.VersionNo;
                ws.Cells[row, 3].Value = item.DevSeason;
                ws.Cells[row, 4].Value = item.DevTeam;
                ws.Cells[row, 5].Value = item.Article;

                ws.Cells[row, 6].Value = item.CwaDeadline;
                ws.Cells[row, 7].Value = item.ModelNo;
                ws.Cells[row, 8].Value = item.ModelName;
                ws.Cells[row, 9].Value = item.OrderStag;
                ws.Cells[row, 10].Value = item.SampleNo;

                ws.Cells[row, 11].Value = item.DevStatus;
                ws.Cells[row, 12].Value = item.DropDate;
                ws.Cells[row, 13].Value = item.HpFlag;
                ws.Cells[row, 14].Value = item.HpSampleNo;
                ws.Cells[row, 15].Value = item.F340SampleNo;

                ws.Cells[row, 16].Value = item.ReleaseType;
                ws.Cells[row, 17].Value = item.CreateDate;
                ws.Cells[row, 18].Value = item.PdmDate;
                ws.Cells[row, 19].Value = item.DevUpDate;
                ws.Cells[row, 20].Value = item.DevBtmDate;

                ws.Cells[row, 21].Value = item.TTUpDate;
                ws.Cells[row, 22].Value = item.TTBtmDate;
                ws.Cells[row, 23].Value = item.ReleaseDate;
                ws.Cells[row, 24].Value = item.TTRejectReason;
                ws.Cells[row, 25].Value = item.TTRejectDate;

                ws.Cells[row, 26].Value = item.TTRejectCount;
                row += 1;
            }
            ws.AutoFitColumns();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        [HttpGet("getF340_Process")]
        public IActionResult GetF340_Process([FromQuery] SF340Schedule sF340Schedule)
        {
            try
            {
                if (sF340Schedule.cwaDateS == "" || sF340Schedule.cwaDateS == null) sF340Schedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
                if (sF340Schedule.cwaDateE == "" || sF340Schedule.cwaDateE == null) sF340Schedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;

                var result = _dksDao.GetF340ProcessView(sF340Schedule);
                Response.AddPagination(result.CurrentPage, result.PageSize,
                result.TotalCount, result.TotalPages);
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
                foreach (var r in result)
                {
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