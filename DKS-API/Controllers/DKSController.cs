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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;

namespace DKS_API.Controllers
{
    public class DKSController : ApiController
    {

        private readonly IDKSDAO _dksDao;
        private readonly IDevBuyPlanDAO _devBuyPlanDAO;
        private readonly IDevTreatmentDAO _devTreatmentDAO;
        public DKSController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IDKSDAO dksDao, IDevBuyPlanDAO devBuyPlanDAO, IDevTreatmentDAO devTreatmentDAO)
        : base(config, webHostEnvironment)
        {
            _dksDao = dksDao;
            _devBuyPlanDAO = devBuyPlanDAO;
            _devTreatmentDAO = devTreatmentDAO;
        }
        [HttpPost("exportF340_Process")]
        public async Task<IActionResult> ExportF340_Process(SF340Schedule sF340Schedule)
        {

            if (sF340Schedule.cwaDateS == "" || sF340Schedule.cwaDateS == null) sF340Schedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340Schedule.cwaDateE == "" || sF340Schedule.cwaDateE == null) sF340Schedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sF340Schedule.cwaDateS = sF340Schedule.cwaDateS.Replace("-", "/");
            sF340Schedule.cwaDateE = sF340Schedule.cwaDateE.Replace("-", "/");
            // query data from database  
            var data = await _dksDao.GetF340ProcessView(sF340Schedule);

            byte[] result = CommonExportReport(data, "TempF340Process.xlsx");

            return File(result, "application/xlsx");
        }

        [HttpGet("getF340_Process")]
        public async Task<IActionResult> GetF340_Process([FromQuery] SF340Schedule sF340Schedule)
        {
            try
            {
                if (sF340Schedule.cwaDateS == "" || sF340Schedule.cwaDateS == null) sF340Schedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
                if (sF340Schedule.cwaDateE == "" || sF340Schedule.cwaDateE == null) sF340Schedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
                sF340Schedule.cwaDateS = sF340Schedule.cwaDateS.Replace("-", "/");
                sF340Schedule.cwaDateE = sF340Schedule.cwaDateE.Replace("-", "/");
                var data = await _dksDao.GetF340ProcessView(sF340Schedule);
                //Response.AddPagination(result.CurrentPage, result.PageSize,
                //result.TotalCount, result.TotalPages);


                PagedList<F340_ProcessDto> result = PagedList<F340_ProcessDto>.Create(data, sF340Schedule.PageNumber, sF340Schedule.PageSize, sF340Schedule.IsPaging);
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
        public async Task<IActionResult> GetBPVersionBySeason(string season, string factory)
        {
            try
            {

                var result = await _devBuyPlanDAO.FindAll(x => x.SEASON.Trim() == season.ToUpper().Trim()
                                                        && x.MANUF.Trim() == factory.ToUpper().Trim())
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
        [HttpGet("getF340_ProcessPpd")]
        public async Task<IActionResult> GetF340_ProcessPpd([FromQuery] SF340PPDSchedule sF340PPDSchedule)
        {
            try
            {
                if (sF340PPDSchedule.cwaDateS == "" || sF340PPDSchedule.cwaDateS == null) sF340PPDSchedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
                if (sF340PPDSchedule.cwaDateE == "" || sF340PPDSchedule.cwaDateE == null) sF340PPDSchedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
                sF340PPDSchedule.cwaDateS = sF340PPDSchedule.cwaDateS.Replace("-", "/");
                sF340PPDSchedule.cwaDateE = sF340PPDSchedule.cwaDateE.Replace("-", "/");
                var data = await _dksDao.GetF340PPDView(sF340PPDSchedule);
                PagedList<F340_PpdDto> result = PagedList<F340_PpdDto>.Create(data, sF340PPDSchedule.PageNumber, sF340PPDSchedule.PageSize, sF340PPDSchedule.IsPaging);
                Response.AddPagination(result.CurrentPage, result.PageSize,
                result.TotalCount, result.TotalPages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}.");
            }
        }
        [HttpPost("exportF340_ProcessPpd")]
        public async Task<IActionResult> ExportF340_ProcessPpd(SF340PPDSchedule sF340PPDSchedule)
        {

            if (sF340PPDSchedule.cwaDateS == "" || sF340PPDSchedule.cwaDateS == null) sF340PPDSchedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340PPDSchedule.cwaDateE == "" || sF340PPDSchedule.cwaDateE == null) sF340PPDSchedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sF340PPDSchedule.cwaDateS = sF340PPDSchedule.cwaDateS.Replace("-", "/");
            sF340PPDSchedule.cwaDateE = sF340PPDSchedule.cwaDateE.Replace("-", "/");
            // query data from database  
            var data = await _dksDao.GetF340PPDView(sF340PPDSchedule);
            var bottom = data.Where(x => x.HpPartNo == "2016").ToList();
            var upper = data.Where(x => x.HpPartNo != "2016").ToList();
            List<object> dataList = new List<object>(){
                bottom,
                upper
            };
            byte[] result = CommonExportReportTabs(dataList, "TempF340PPDProcessTabs.xlsx");

            return File(result, "application/xlsx");
        }

        [HttpPost("editPicF340Ppd")]
        public async Task<IActionResult> EditPicF340Ppd()
        {
            try
            {
                var sampleNo = HttpContext.Request.Form["sampleNo"].ToString();
                var treatMent = HttpContext.Request.Form["treatMent"].ToString();
                var partName = HttpContext.Request.Form["partName"].ToString();
                var article = HttpContext.Request.Form["article"].ToString();
                var partNo = partName.Trim().Split(" ")[0];
                var treatMentNo = treatMent.Trim().Split(" ")[0];
                var fileName = string.Format("{0}_{1}_{2}.jpg", article, partNo, treatMentNo);

                DevTreatment model = _devTreatmentDAO.FindSingle(
                                 x => x.SAMPLENO.Trim() == sampleNo.Trim() &&
                                 x.PARTNO.Trim() == partNo &&
                                 x.TREATMENTCODE.Trim() == treatMentNo);

                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var file = HttpContext.Request.Form.Files[0];
                    if (await SaveFiletoServer(file, "F340PpdPic", fileName))
                    {
                        model.PHOTO = fileName;
                        _devTreatmentDAO.Update(model);
                    }
                }
                else
                {   //do CRUD-D here.

                    if (await SaveFiletoServer(null, "F340PpdPic", fileName))
                    {
                        model.PHOTO = "";
                        _devTreatmentDAO.Update(model);
                    }
                }
                await _devTreatmentDAO.SaveAll();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpPost("editF340Ppds")]
        public async Task<IActionResult> EditF340Ppds(List<F340_PpdDto> dtos)
        {
            var editCount = 0;
            try
            {
                var content = "";
                foreach (F340_PpdDto dto in dtos)
                {
                    if (dto.PartName.Trim().Length < 1 || dto.TreatMent.Trim().Length < 1 || dto.ReleaseType.Trim() != "CWA") continue;
                    var partNo = dto.PartName.Trim().Split(" ")[0];
                    var treatMentNo = dto.TreatMent.Trim().Split(" ")[0];
                    DevTreatment model = _devTreatmentDAO.FindSingle(
                                                     x => x.SAMPLENO.Trim() == dto.SampleNo.Trim() &&
                                                     x.PARTNO.Trim() == partNo &&
                                                     x.TREATMENTCODE.Trim() == treatMentNo);

                    if (model != null)
                    {
                        if (model.PPD_REMARK.ToSafetyString().Trim() == dto.PpdRemark.ToSafetyString().Trim()) continue;
                        model.PPD_REMARK = dto.PpdRemark.Trim();
                        _devTreatmentDAO.Update(model);
                        editCount++;
                        content += model.ARTICLE;
                        content += "、";
                    }
                }

                if (editCount > 0)
                {
                    content = content.Remove(content.Length - 1);
                    var toMails = new List<string>();
                    var users = await _dksDao.GetUsersByRole("GM0000000038");
                    users.ForEach(x =>
                    {
                        toMails.Add(x.EMAIL);
                    });
                    await SendListMailAsync(toMails, "These Article Add Memo Please check !", content, null);
                    await _devTreatmentDAO.SaveAll();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(editCount);

        }
        [HttpPost("editF340Ppd/{type}")]
        public async Task<IActionResult> EditF340Ppd(F340_PpdDto dto,string type)
        {
            var isSuccess = false;
            try
            {
                    if (dto.PartName.Trim().Length < 1 || dto.TreatMent.Trim().Length < 1 || dto.ReleaseType.Trim() != "CWA") return Ok(isSuccess);
                    var partNo = dto.PartName.Trim().Split(" ")[0];
                    var treatMentNo = dto.TreatMent.Trim().Split(" ")[0];
                    DevTreatment model = _devTreatmentDAO.FindSingle(
                                                     x => x.SAMPLENO.Trim() == dto.SampleNo.Trim() &&
                                                     x.PARTNO.Trim() == partNo &&
                                                     x.TREATMENTCODE.Trim() == treatMentNo);

                    if (model != null)
                    {
                        // type is here
                        if( type == "PhotoComment")model.PHOTO_COMMENT = dto.PhotoComment.Trim();
                        if( type == "PpdRemark")model.PPD_REMARK = dto.PpdRemark.Trim();
                        
                        
                        _devTreatmentDAO.Update(model);
                    }

                    await _devTreatmentDAO.SaveAll();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(isSuccess);

        }

        ////// It is a lazy way because I don't wanna craeat Servervice Layer......//////




    }
}