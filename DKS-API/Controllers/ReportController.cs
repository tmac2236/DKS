using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Drawing;
using Microsoft.Extensions.Logging;
using DKS_API.Data;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using DKS_API.Helpers;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Controllers
{
    public class ReportController : ApiController
    {
        private readonly DataContext _context;
        private readonly IReporDAO _reporDAO;
        private readonly ISPFactoryDAO _sPFactoryDAO;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IConfiguration config, IWebHostEnvironment webHostEnvironment,DataContext context, IReporDAO reporDAO, ISPFactoryDAO sPFactoryDAO , ILogger<ReportController> logger)
                 : base(config, webHostEnvironment)
        {
            _context = context;
            _reporDAO = reporDAO;
            _logger = logger;
            _sPFactoryDAO = sPFactoryDAO;
        }
        [HttpGet("throw")]
        public object Throw()
        {
            throw new Exception();
        }

        [HttpGet("exportGetNoOperaionList")]
        public async Task<IActionResult> exportGetNoOperaionList(string startDate)
        {

            // query data from database  
            var data = await GetNoOperaionListFromFirst2Date(startDate);

            WorkbookDesigner designer = new WorkbookDesigner();
            #region style設計
            //Create style object(標題用)
            Style titleStyle = designer.Workbook.CreateStyle();
            titleStyle.Pattern = BackgroundType.Solid;
            titleStyle.ForegroundColor = Color.Green;
            //Create style flag(標題用)
            StyleFlag titleFlag = new StyleFlag();
            titleFlag.CellShading = true;
            #endregion style設計

            Worksheet ws = designer.Workbook.Worksheets[0];

            int titlenum = 0;
            ws.Cells[0, titlenum].Value = "線別-日期";

            data[0].Count();    //use for 68 lean loop

            #region title generate dynamiclly
            foreach (List<NoOperationDto> list in data)
            {
                if (list[0].StartDate == null) continue;
                titlenum += 1;
                ws.Cells[0, titlenum].Value = list[0].StartDate;

                Cell cell = ws.Cells[0, titlenum];
                Style style = cell.GetStyle();
                style.Custom = "yyyy/mm/dd";
                //Apply the style to cell.
                cell.SetStyle(style);
            }
            titlenum += 1;
            ws.Cells[0, titlenum].Value = "Total";
            titlenum += 1;
            ws.Cells[0, titlenum].Value = "Score(%)";

            ws.Cells.Rows[0].ApplyStyle(titleStyle, titleFlag);
            #endregion  title generate dynamiclly

            //第二列以後
            int leanCount = data[0].Count();    //loop 68 times for leans
            int workDaysCount = data.Count(); //loop day exclude total and percent
            int rowNum = 1;

            //foreach (NoOperationDto obj in data[0]) //loop 68 times for leans
            for (int i = 0; i < leanCount; i++)
            {
                //迴圈跑天數(扣除非上班日)
                int column = 0;
                //每一筆row的Column是Lean名稱
                string leanObj = data[0][rowNum - 1].Lean;
                ws.Cells[rowNum, column].Value = leanObj;
                column += 1;
                //Debug.WriteLine("*******i =" + i);
                for (int j = 0; j < workDaysCount; j++)
                {
                    //Debug.WriteLine("j =" + j);
                    decimal? statusObj = data[j][i].Status;
                    ws.Cells[rowNum, column].Value = statusObj;
                    column += 1;
                }
                rowNum += 1;
            }
            ws.AutoFitColumns();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        //http://localhost:5000/api/report/getReportDataPass
        [HttpPost("getReportDataPass")]
        public async Task<IActionResult> GetReportDataPass(SReportDataPassDto sReportDataPassDto)
        {
            var data = await _reporDAO.GetReportDataPass(sReportDataPassDto);
            return Ok(data);

        }
        [HttpGet("exportGetReportDataPass")]
        public async Task<IActionResult> exportGetReportDataPass
        (string lineID, string modelName, string model, string checkPass, string cDate, string cDateE)
        {
            SReportDataPassDto sReportDataPassDto = new SReportDataPassDto();
            if (lineID != "undefined") sReportDataPassDto.LineID = lineID;
            if (modelName != "undefined") sReportDataPassDto.ModelName = modelName;
            if (model != "undefined") sReportDataPassDto.Model = model;
            if (checkPass != "undefined") sReportDataPassDto.CheckPass = checkPass;
            if (cDate != "undefined") sReportDataPassDto.CDate = cDate;
            if (cDateE != "undefined") sReportDataPassDto.CDateE = cDateE;
            // query data from database  
            //var data = await _reporDAO.GetReportDataPass(sReportDataPassDto);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\GetReportDataPass.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            //designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        [HttpPost("getQueryPDModel")]
        public async Task<IActionResult> GetQueryPDModel(SPDModelDto sPDModelDto)
        {
            var data = await _reporDAO.GetPDModels(sPDModelDto);
            return Ok(data);

        }
        [HttpGet("exportGetQueryPDModel")]
        public async Task<IActionResult> exportGetQueryPDModel
        (string startDate, string endDate, string teamID)
        {
            SPDModelDto sPDModelDto = new SPDModelDto();
            sPDModelDto.StartDate = startDate;
            sPDModelDto.EndDate = endDate;
            sPDModelDto.TeamID = teamID == null ? "" : teamID;
            // query data from database  
            var data = await _reporDAO.GetPDModels(sPDModelDto);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\GetQuery_PD_Model.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        [HttpGet("getAttendanceList")]
        public IActionResult GetAttendanceList([FromQuery] SAttendanceDto sAttendanceDto)
        {
            var data = _reporDAO.GetAttendances(sAttendanceDto);

            Response.AddPagination(data.CurrentPage, data.PageSize,
                 data.TotalCount, data.TotalPages);

            return Ok(data);
        }

        [HttpGet("exportGetAttendanceList")]
        public async Task<IActionResult> exportGetAttendanceList()
        {
            // query data from database  
            var data = await _reporDAO.GetAttendances();

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\GetAttendanceList.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }
        [HttpPost("getChangeWorkers")]
        public async Task<IActionResult> GetChangeWorkers(SPDModelDto sPDModelDto)
        {
            var data = await _reporDAO.GetChangeWorkers(sPDModelDto);
            return Ok(data);

        }
        [HttpGet("exportGetChangeWorkers")]
        public async Task<IActionResult> exportGetChangeWorkers
        (string startDate, string endDate, string teamID)
        {
            SPDModelDto sPDModelDto = new SPDModelDto();
            if (startDate != "undefined") sPDModelDto.StartDate = startDate;
            if (endDate != "undefined") sPDModelDto.EndDate = endDate;
            if (teamID != "undefined") sPDModelDto.TeamID = teamID;
            // query data from database  
            var data = await _reporDAO.GetChangeWorkers(sPDModelDto);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\GetChangeWorkers.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }

        [HttpGet("getNoOperation")]
        public async Task<IActionResult> GetNoOperationDuringMonth(string startDate)
        {
            var nastedList = await GetNoOperaionListFromFirst2Date(startDate);

            return Ok(nastedList);
        }
        //取得該工廠的有效線名
        [HttpGet("getAllLean")]
        public async Task<IActionResult> GetAllLeanNo()
        {
            var data = await _sPFactoryDAO.GetAllLeans();

            return Ok(data);
        }
        //取得該工廠的ModleNo By LeanId
        [HttpGet("getModelByLean")]
        public async Task<IActionResult> GetModelByLean(string lean)
        {
            var data = await _sPFactoryDAO.GetModelsByLean(lean);

            return Ok(data);
        }
        
        //http://localhost:5000/api/report/getPlan
        [HttpGet("getPlan")]
        public async Task<IActionResult> GetTestJustName()
        {
            var deptList = from t1 in _context.SampleWorkBase
                           join t2 in _context.SampleWorkProcess on t1.Fid equals t2.Hid
                           join t3 in _context.SampleWorkWorker on t2.Fid equals t3.Hid
                           where t1.SampleNr != null && t1.Maker != null
                           select new PlanProcessWorkDto
                           {
                               Maker = t1.Maker,
                               SampleNr = t1.SampleNr,
                               SampleDesc = t1.SampleDesc,
                               OrdinalNum = t2.OrdinalNum,
                               ProcessNum = t2.ProcessNum,
                               Description = t2.Description,
                               WorkerNum = t3.WorkerNum,
                               WorkerName = t3.WorkerName
                           };
            var list = await deptList.ToListAsync().ConfigureAwait(false); // <-- notice the `await`

            return Ok(list);
        }
        //http://localhost:5000/api/planWorker/getOnePlan?sampleNr=LAF12&maker=L2B
        [HttpGet("getOnePlan")]
        public async Task<IActionResult> GetPlanProcessWorkByLeanNModelNo(string sampleNr, string maker)
        {
            var deptList = from t1 in _context.SampleWorkBase
                           join t2 in _context.SampleWorkProcess on t1.Fid equals t2.Hid
                           join t3 in _context.SampleWorkWorker on t2.Fid equals t3.Hid
                           where t1.SampleNr == sampleNr && t1.Maker == maker
                           select new PlanProcessWorkDto
                           {
                               Maker = t1.Maker,
                               SampleNr = t1.SampleNr,
                               SampleDesc = t1.SampleDesc,
                               OrdinalNum = t2.OrdinalNum,
                               ProcessNum = t2.ProcessNum,
                               Description = t2.Description,
                               WorkerNum = t3.WorkerNum,
                               WorkerName = t3.WorkerName
                           };
            var list = await deptList.ToListAsync().ConfigureAwait(false); // <-- notice the `await`

            return Ok(list);
        }

        public async Task<List<List<NoOperationDto>>> GetNoOperaionListFromFirst2Date(string startDate)
        {
            List<List<NoOperationDto>> nastedList = new List<List<NoOperationDto>>();
            DateTime theDate = Convert.ToDateTime(startDate);
            int theEndDay = theDate.Day;
            for (int i = 1; i <= theEndDay; i++)
            {
                DateTime itemDate = new DateTime(theDate.Year, theDate.Month, i);
                string dateStr = itemDate.ToString("yyyy-MM-dd");
                List<NoOperationDto> data = await _reporDAO.GetNoOperations(dateStr);
                if (data[0].OnDuty.ToInt() > 0)
                { //onduty=1 上班日,  onduty=0假日(全廠沒有實際工時)
                    nastedList.Add(data);
                }
            }

            int parent = nastedList.Count();
            //計算累計分數
            List<NoOperationDto> calNumList = new List<NoOperationDto>();
            //計算Percent
            List<NoOperationDto> scoreList = new List<NoOperationDto>();

            for (int i = 0; i < nastedList[0].Count(); i++)
            {
                decimal calNum = 0;
                for (int j = 0; j < parent; j++)
                { //跑N天的迴圈
                    var item = nastedList[j]; //第N天
                    calNum += item[i].Status.ToDecimal(); //該N天的List的i個Lean線做累加
                }
                NoOperationDto res = new NoOperationDto();
                res.StartDate = null;
                res.Status = calNum;
                calNumList.Add(res);

                decimal score = 0;
                NoOperationDto scoreRes = new NoOperationDto();
                try
                {
                    decimal num = (decimal)calNum / (decimal)parent;
                    score = Decimal.Round(((num) * 100), 2);
                }
                catch (Exception ex) { }
                scoreRes.StartDate = null;
                scoreRes.Status = score;
                scoreList.Add(scoreRes);
            }
            nastedList.Add(calNumList);
            nastedList.Add(scoreList);

            return nastedList;
        }

    }
}