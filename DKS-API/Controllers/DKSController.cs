using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Data.Interface;
using Aspose.Cells;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DKS_API.Helpers;
using Microsoft.AspNetCore.Hosting;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace DKS_API.Controllers
{
    public class DKSController : ApiController
    {
        private readonly IDKSDAO _dksDao;
        private readonly IDevBuyPlanDAO _devBuyPlanDAO;
        private readonly IDevTreatmentDAO _devTreatmentDAO;
        private readonly IDevTreatmentFileDAO _devTreatmentFileDAO;
        private readonly IDevSysSetDAO _devSysSetDAO;
        private readonly ISendMailService _sendMailService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        public DKSController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<DKSController> logger, IDKSDAO dksDao, IDevBuyPlanDAO devBuyPlanDAO, IDevTreatmentDAO devTreatmentDAO,
        IDevTreatmentFileDAO devTreatmentFileDAO, IDevSysSetDAO devSysSetDAO, ISendMailService sendMailService, IFileService fileService, IExcelService excelService)
        : base(config, webHostEnvironment, logger)
        {
            _sendMailService = sendMailService;
            _fileService = fileService;
            _excelService = excelService;
            _dksDao = dksDao;
            _devBuyPlanDAO = devBuyPlanDAO;
            _devTreatmentDAO = devTreatmentDAO;
            _devTreatmentFileDAO = devTreatmentFileDAO;
            _devSysSetDAO = devSysSetDAO;


        }
        [HttpPost("exportF340_Process")]
        public async Task<IActionResult> ExportF340_Process(SF340Schedule sF340Schedule)
        {
            _logger.LogInformation(String.Format(@"****** DKSController ExportF340_Process fired!! ******"));
            if (sF340Schedule.cwaDateS == "" || sF340Schedule.cwaDateS == null) sF340Schedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340Schedule.cwaDateE == "" || sF340Schedule.cwaDateE == null) sF340Schedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sF340Schedule.cwaDateS = sF340Schedule.cwaDateS.Replace("-", "/");
            sF340Schedule.cwaDateE = sF340Schedule.cwaDateE.Replace("-", "/");
            // query data from database  
            var data = await _dksDao.GetF340ProcessView(sF340Schedule);

            byte[] result = _excelService.CommonExportReport(data, "TempF340Process.xlsx");

            return File(result, "application/xlsx");
        }

        [HttpGet("getF340_Process")]
        public async Task<IActionResult> GetF340_Process([FromQuery] SF340Schedule sF340Schedule)
        {
            _logger.LogInformation(String.Format(@"****** DKSController GetF340_Process fired!! ******"));

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

        [HttpPost("checkF420Valid")]
        public IActionResult CheckF420Valid([FromForm] ArticlePic excel)
        {
            _logger.LogInformation(String.Format(@"****** DKSController CheckF420Valid fired!! ******"));

            int processIndex = 0;//use in debug

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
        [HttpGet("getF340_ProcessPpd")]
        public async Task<IActionResult> GetF340_ProcessPpd([FromQuery] SF340PPDSchedule sF340PPDSchedule)
        {
            _logger.LogInformation(String.Format(@"****** DKSController GetF340_ProcessPpd fired!! ******"));

            if (sF340PPDSchedule.cwaDateS == "" || sF340PPDSchedule.cwaDateS == null) sF340PPDSchedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340PPDSchedule.cwaDateE == "" || sF340PPDSchedule.cwaDateE == null) sF340PPDSchedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sF340PPDSchedule.cwaDateS = sF340PPDSchedule.cwaDateS.Replace("-", "/");
            sF340PPDSchedule.cwaDateE = sF340PPDSchedule.cwaDateE.Replace("-", "/");
            var data = await _dksDao.GetF340PPDView(sF340PPDSchedule);
            if (sF340PPDSchedule.ubType == "U") data = data.Where(x => x.HpPartNo != "2016").ToList();
            if (sF340PPDSchedule.ubType == "B") data = data.Where(x => x.HpPartNo == "2016").ToList();
            PagedList<F340_PpdDto> result = PagedList<F340_PpdDto>.Create(data, sF340PPDSchedule.PageNumber, sF340PPDSchedule.PageSize, sF340PPDSchedule.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }
        [HttpPost("exportF340_ProcessPpd")]
        public async Task<IActionResult> ExportF340_ProcessPpd(SF340PPDSchedule sF340PPDSchedule)
        {
            _logger.LogInformation(String.Format(@"****** DKSController ExportF340_ProcessPpd fired!! ******"));

            var dksSignature = _config.GetSection("AppSettings:encodeStr").Value;
            if (sF340PPDSchedule.cwaDateS == "" || sF340PPDSchedule.cwaDateS == null) sF340PPDSchedule.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (sF340PPDSchedule.cwaDateE == "" || sF340PPDSchedule.cwaDateE == null) sF340PPDSchedule.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sF340PPDSchedule.cwaDateS = sF340PPDSchedule.cwaDateS.Replace("-", "/");
            sF340PPDSchedule.cwaDateE = sF340PPDSchedule.cwaDateE.Replace("-", "/");
            // query data from database  
            var data = await _dksDao.GetF340PPDView(sF340PPDSchedule);
            //
            data.ForEach(x =>
            {
                if (x.Photo.Length > 1)
                {
                    var factoryApi = "";
                    var apiUrl = _config.GetSection("AppSettings:ApiUrl").Value;
                    switch (sF340PPDSchedule.factory)
                    {
                        case "C": //SHC
                            factoryApi = apiUrl + "dks/getF340PpdPic?isStanHandsome=";
                            break;
                        case "E": //CB
                            factoryApi = "http://10.9.0.35/material/WatermarkAPI/GetF340PpdPic?param=";
                            break;
                        case "D": //SPC
                            factoryApi = "http://10.10.0.21/material/WatermarkAPI/GetF340PpdPic?param=";
                            break;
                        case "U": //TSH
                            factoryApi = "http://10.11.0.22/material/WatermarkAPI/GetF340PpdPic?param=";
                            break;
                        default:
                            {
                                factoryApi = apiUrl + "dks/getF340PpdPic?isStanHandsome=";
                                break;
                            }
                    }
                    var param = dksSignature + x.DevSeason + "$" + x.Article + "$" + x.Photo + "$" + sF340PPDSchedule.factory + "$" + sF340PPDSchedule.loginUser;
                    var encodeStr = CSharpLab.Btoa(param);
                    var dataUrl = string.Format(@"{0}{1}", factoryApi, encodeStr);
                    //let dataUrl = environment.apiUrl + "dks/getF340PpdPdf?isStanHandsome=" + window.btoa(param);
                    //x.Photo = "=HYPERLINK(\"" + dataUrl + "\",\"jpg\")";
                    x.Photo = dataUrl;

                    //x.Photo = "http://" + serverIp + "/assets/F340PpdPic/" + x.DevSeason + "/" + x.Article + "/" + x.Photo;
                }
                if (x.Pdf.Length > 1)
                {   //encoding version
                    var factoryApi = "";
                    var apiUrl = _config.GetSection("AppSettings:ApiUrl").Value;
                    switch (sF340PPDSchedule.factory)
                    {
                        case "C": //SHC
                            factoryApi = apiUrl + "dks/getF340PpdPdf?isStanHandsome=";
                            break;
                        case "E": //CB
                            factoryApi = "http://10.9.0.35/material/WatermarkAPI/GetF340PpdPdf?param=";
                            break;
                        case "D": //SPC
                            factoryApi = "http://10.10.0.21/material/WatermarkAPI/GetF340PpdPdf?param=";
                            break;
                        case "U": //TSH
                            factoryApi = "http://10.11.0.22/material/WatermarkAPI/GetF340PpdPdf?param=";
                            break;
                        default:
                            {
                                factoryApi = apiUrl + "dks/getF340PpdPdf?isStanHandsome=";
                                break;
                            }
                    }
                    var param = dksSignature + x.DevSeason + "$" + x.Article + "$" + x.Pdf + "$" + sF340PPDSchedule.factory + "$" + sF340PPDSchedule.loginUser;
                    var encodeStr = CSharpLab.Btoa(param);
                    var dataUrl = string.Format(@"{0}{1}", factoryApi, encodeStr);

                    // no encoding version
                    //var dataUrl = string.Format(@"{0}:{1}{2}{3}/{4}/{5}",ip,spaPort,"/assets/F340PpdPic/",x.DevSeason,x.Article,x.Pdf);
                    x.Pdf = dataUrl;

                }
                if (x.FgtFileName.Length > 1)
                {
                    var factoryApi = "";

                    switch (sF340PPDSchedule.factory)
                    {
                        case "C": //SHC
                            factoryApi = "http://10.4.0.39:6970/assets/F340PpdPic/QCTestResult/";
                            break;
                        case "E": //CB
                            factoryApi = "http://10.9.0.35/material/Upload/F340CmptMatPic/QCTestResult/";
                            break;
                        case "D": //SPC
                            factoryApi = "http://10.10.0.21/material/Upload/F340CmptMatPic/QCTestResult/";
                            break;
                        case "U": //TSH
                            factoryApi = "http://10.11.0.22/material/Upload/F340CmptMatPic/QCTestResult/";
                            break;
                        default:
                            factoryApi = "http://10.4.0.39:6970/assets/F340PpdPic/QCTestResult/";
                            break;
                    }

                    var dataUrl = string.Format(@"{0}{1}/{2}", factoryApi, x.Article, x.FgtFileName);
                    x.FgtFileName = dataUrl;
                }
            });
            var upper = data.Where(x => x.HpPartNo != "2016").ToList();
            var bottom = data.Where(x => x.HpPartNo == "2016").ToList();

            List<List<F340_PpdDto>> dataList = new List<List<F340_PpdDto>>(){
                upper,
                bottom
            };
            byte[] result = _excelService.CommonExportReportTabs4F340PPD(dataList, "TempF340PPDProcessTabs.xlsx");

            return File(result, "application/xlsx");
        }
        //上傳F340PPD圖片或刪除圖片
        [HttpPost("editPicF340Ppd")]
        public async Task<IActionResult> EditPicF340Ppd()
        {
            _logger.LogInformation(String.Format(@"****** DKSController EditPicF340Ppd fired!! ******"));

            var sampleNo = HttpContext.Request.Form["sampleNo"].ToString().Trim();
            var treatMent = HttpContext.Request.Form["treatMent"].ToString().Trim();
            var partName = HttpContext.Request.Form["partName"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var devSeason = HttpContext.Request.Form["devSeason"].ToString().Trim();
            var fileName = HttpContext.Request.Form["photo"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            var partNo = partName.Split(" ")[0];
            var treatMentNo = treatMent.Split(" ")[0];

            DateTime nowtime = DateTime.Now;
            var updateTimeStr = nowtime.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime updateTime = updateTimeStr.ToDateTime();


            if (fileName == "")
            {
                //fileName + yyyy_MM_dd_HH_mm_ss_
                var formateDate = nowtime.ToString("yyyyMMddHHmmss");
                fileName = string.Format("{0}_{1}_{2}_{3}.jpg", article, partNo, treatMentNo, formateDate);
            }

            List<string> nastFileName = new List<string>();
            nastFileName.Add(devSeason);
            nastFileName.Add(article);
            nastFileName.Add(fileName);

            DevTreatment model = _devTreatmentDAO.FindSingle(
                             x => x.SAMPLENO.Trim() == sampleNo.Trim() &&
                             x.PARTNO.Trim() == partNo &&
                             x.TREATMENTCODE.Trim() == treatMentNo &&
                             x.FACTORYID == factoryId);

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];
                //IformFile  ==> img ==>byte array ==> IformFile
                Image image = Image.FromStream(file.OpenReadStream(), true, true);
                var newImage = new Bitmap(1024, 768);
                using (var g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, 0, 0, 1024, 768);
                }
                ImageConverter converter = new ImageConverter();
                byte[] bt = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
                var stream = new MemoryStream(bt);
                IFormFile resizeFile = new FormFile(stream, 0, bt.Length, file.Name, file.FileName);
                if (await _fileService.SaveFiletoServer(resizeFile, "F340PpdPic", nastFileName))
                {
                    model.PHOTO = fileName;
                    _devTreatmentDAO.Update(model);

                    DevTreatmentFile opRecord = new DevTreatmentFile();
                    opRecord.ARTICLE = article;
                    opRecord.PARTNO = partNo;
                    opRecord.TREATMENTCODE = treatMentNo;
                    opRecord.FILE_NAME = fileName;
                    opRecord.KIND = "1";// 1: JPG 2:PDF
                    opRecord.FILE_COMMENT = "";
                    opRecord.UPUSR = loginUser;
                    opRecord.UPTIME = updateTime;
                    _devTreatmentFileDAO.Add(opRecord);
                    _logger.LogInformation(String.Format(@"******DKSController EditPicF340Ppd Add a Picture: {0}!! ******", fileName));
                }
            }
            else
            {   //do CRUD-D here.

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    model.PHOTO = "";
                    _devTreatmentDAO.Update(model);

                    DevTreatmentFile opRecord = _devTreatmentFileDAO.FindSingle(
                    x => x.FILE_NAME.Trim() == fileName.Trim());
                    _devTreatmentFileDAO.Remove(opRecord);
                    _logger.LogInformation(String.Format(@"******DKSController EditPicF340Ppd Delete a Picture: {0}!! ******", fileName));
                }
            }
            await _devTreatmentDAO.SaveAll();

            await _devTreatmentFileDAO.SaveAll();
            return Ok(model);
        }
        //上傳F340PPD PDF或刪除PDF
        [HttpPost("editPdfF340Ppd")]
        public async Task<IActionResult> EditPdfF340Ppd()
        {
            _logger.LogInformation(String.Format(@"****** DKSController EditPdfF340Ppd fired!! ******"));

            var sampleNo = HttpContext.Request.Form["sampleNo"].ToString().Trim();
            var treatMent = HttpContext.Request.Form["treatMent"].ToString().Trim();
            var partName = HttpContext.Request.Form["partName"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var devSeason = HttpContext.Request.Form["devSeason"].ToString().Trim();
            var fileName = HttpContext.Request.Form["pdf"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            var partNo = partName.Split(" ")[0];
            var treatMentNo = treatMent.Split(" ")[0];

            DateTime nowtime = DateTime.Now;
            var updateTimeStr = nowtime.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime updateTime = updateTimeStr.ToDateTime();

            if (fileName == "")
            {
                //fileName + yyyy_MM_dd_HH_mm_ss_
                var formateDate = nowtime.ToString("yyyyMMddHHmmss");
                fileName = string.Format("{0}_{1}_{2}_{3}.pdf", article, partNo, treatMentNo, formateDate);
            }

            List<string> nastFileName = new List<string>();
            nastFileName.Add(devSeason);
            nastFileName.Add(article);
            nastFileName.Add(fileName);

            DevTreatment model = _devTreatmentDAO.FindSingle(
                             x => x.SAMPLENO.Trim() == sampleNo.Trim() &&
                             x.PARTNO.Trim() == partNo &&
                             x.TREATMENTCODE.Trim() == treatMentNo &&
                             x.FACTORYID == factoryId);

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];
                if (await _fileService.SaveFiletoServer(file, "F340PpdPic", nastFileName))
                {
                    model.PDF = fileName;
                    _devTreatmentDAO.Update(model);

                    DevTreatmentFile opRecord = new DevTreatmentFile();
                    opRecord.ARTICLE = article;
                    opRecord.PARTNO = partNo;
                    opRecord.TREATMENTCODE = treatMentNo;
                    opRecord.FILE_NAME = fileName;
                    opRecord.KIND = "2";// 1: JPG 2:PDF
                    opRecord.FILE_COMMENT = "";
                    opRecord.UPUSR = loginUser;
                    opRecord.UPTIME = updateTime;
                    _devTreatmentFileDAO.Add(opRecord);
                    _logger.LogInformation(String.Format(@"******DKSController EditPdfF340Ppd Add a Pdf: {0}!! ******", fileName));
                }
            }
            else
            {   //do CRUD-D here.

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    model.PDF = "";
                    _devTreatmentDAO.Update(model);

                    DevTreatmentFile opRecord = _devTreatmentFileDAO.FindSingle(
                    x => x.FILE_NAME.Trim() == fileName.Trim());
                    _devTreatmentFileDAO.Remove(opRecord);
                    _logger.LogInformation(String.Format(@"******DKSController EditPdfF340Ppd Delete a Pdf: {0}!! ******", fileName));
                }
            }
            await _devTreatmentDAO.SaveAll();

            await _devTreatmentFileDAO.SaveAll();

            return Ok(model);
        }
        /* Pending
        [HttpPost("editF340Ppds")]
        public async Task<IActionResult> EditF340Ppds(List<F340_PpdDto> dtos)
        {

            _logger.LogInformation(String.Format(@"******DKSController EditF340Ppds fired!! ******"));

            var editCount = 0;
            var content = "";
            foreach (F340_PpdDto dto in dtos)
            {
                if (dto.PartName.Trim().Length < 1 || dto.TreatMent.Trim().Length < 1 || dto.ReleaseType.Trim() != "CWA") continue;
                var partNo = dto.PartName.Trim().Split(" ")[0];
                var treatMentNo = dto.TreatMent.Trim().Split(" ")[0];
                DevTreatment model = _devTreatmentDAO.FindSingle(
                                                 x => x.SAMPLENO.Trim() == dto.SampleNo.Trim() &&
                                                 x.PARTNO.Trim() == partNo &&
                                                 x.TREATMENTCODE.Trim() == treatMentNo &&
                                                 x.FACTORYID == factoryId);

                if (model != null)
                {
                    if (model.PPD_REMARK.ToSafetyString().Trim() == dto.PpdRemark.ToSafetyString().Trim()) continue;
                    _logger.LogInformation(String.Format(@"******DKSController EditF340Ppds Add a Remark: {0}!! ******", dto.PpdRemark));
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
                await _sendMailService.SendListMailAsync(toMails, null, "These Article Add Memo Please check !", content, null);
                await _devTreatmentDAO.SaveAll();
            }
            return Ok(editCount);

        }
        */
        [HttpPost("editF340Ppd/{type}")]
        public async Task<IActionResult> EditF340Ppd(F340_PpdDto dto, string type)
        {

            _logger.LogInformation(String.Format(@"******DKSController EditF340Ppd fired!! ******"));

            var isSuccess = false;

            if (dto.PartName.Trim().Length < 1 || dto.TreatMent.Trim().Length < 1 || dto.ReleaseType.Trim() != "CWA") return Ok(isSuccess);
            var partNo = dto.PartName.Trim().Split(" ")[0];
            var treatMentNo = dto.TreatMent.Trim().Split(" ")[0];
            DevTreatment model = _devTreatmentDAO.FindSingle(
                                             x => x.SAMPLENO.Trim() == dto.SampleNo.Trim() &&
                                             x.PARTNO.Trim() == partNo &&
                                             x.TREATMENTCODE.Trim() == treatMentNo &&
                                             x.FACTORYID == dto.Factory);

            if (model != null)
            {
                // type is here
                if (type == "PhotoComment")
                {
                    model.PHOTO_COMMENT = dto.PhotoComment.Trim();
                    _logger.LogInformation(String.Format(@"******DKSController EditF340Ppd Add a PhotoComment: {0}!! ******", dto.PhotoComment));
                }
                if (type == "PpdRemark")
                {
                    model.PPD_REMARK = dto.PpdRemark.Trim();
                    _logger.LogInformation(String.Format(@"******DKSController EditF340Ppd Add a PpdRemark: {0}!! ******", dto.PhotoComment));
                }

                _devTreatmentDAO.Update(model);
            }

            await _devTreatmentDAO.SaveAll();
            return Ok(isSuccess);

        }
        [HttpPost("sentMailF340PpdByArticle")]
        public async Task<IActionResult> SentMailF340PpdByArticle(SF340PPDSchedule sF340PPDSchedule)
        {

            _logger.LogInformation(String.Format(@"******DKSController SentMailF340PpdByArticle fired!! ******"));

            //var dksSignature = _config.GetSection("DksSignatureLine").Value;
            var dksSignature = "";
            var content = string.Format(@"The Article : {0} Added Memo please check it in F340-PPD of the below website.{1}", sF340PPDSchedule.article, dksSignature);

            var toMails = new List<string>();
            var users = await _dksDao.GetUsersByRole("GM0000000038");
            users.ForEach(x =>
            {
                toMails.Add(x.EMAIL);
            });
            await _sendMailService.SendListMailAsync(toMails, null, "This Article Add Memo Please check it in F340-PPD !", content, null);
            return Ok();

        }
        [HttpGet("getF340PpdPic")]
        public IActionResult getF340PpdPic(string isStanHandsome)
        {
            _logger.LogInformation(String.Format(@"****** DKSController getF340PpdPic fired!! ******"));

            var decodeStr = CSharpLab.Atob(isStanHandsome);
            var dksSignature = _config.GetSection("AppSettings:encodeStr").Value;
            decodeStr = decodeStr.Replace(dksSignature, "");
            List<string> nastFileName = decodeStr.Split("$").ToList();
            var factory = nastFileName[nastFileName.Count - 2];
            nastFileName.RemoveAt(nastFileName.Count - 2);
            var loginUser = nastFileName[nastFileName.Count - 1];
            nastFileName.RemoveAt(nastFileName.Count - 1);
            var pathList = _fileService.GetLocalPath("F340PpdPic", nastFileName);

            var result = _fileService.GetByteArrayByLocalUrlAddWaterMask(pathList[1], loginUser);
            return File(result, "image/jpeg");//"image/jpeg"  "application/pdf"
        }
        [HttpGet("getF340PpdPdf")]
        public IActionResult getF340PpdPdf(string isStanHandsome)
        {
            _logger.LogInformation(String.Format(@"****** DKSController getF340PpdPdf fired!! ******"));

            var decodeStr = CSharpLab.Atob(isStanHandsome);
            var dksSignature = _config.GetSection("AppSettings:encodeStr").Value;
            decodeStr = decodeStr.Replace(dksSignature, "");
            List<string> nastFileName = decodeStr.Split("$").ToList();
            var factory = nastFileName[nastFileName.Count - 2];
            nastFileName.RemoveAt(nastFileName.Count - 2);
            var loginUser = nastFileName[nastFileName.Count - 1];
            nastFileName.RemoveAt(nastFileName.Count - 1);

            var pathList = _fileService.GetLocalPath("F340PpdPic", nastFileName);

            var result = System.IO.File.ReadAllBytes(pathList[1]);
            //var result =  _fileService.AddWatermarkPdf(pathList[1],loginUser);

            return File(result, "application/pdf");//"image/jpeg"  "application/pdf"
        }
        [HttpGet("rejectF340Process")]
        public async Task<IActionResult> RejectF340Process(string sampleNo, string type)
        {

            _logger.LogInformation(String.Format(@"****** DKSController RejectF340Process fired!! ******"));

            string errMsg = "";
            if (!(type == "U" || type == "B" || type == "UB"))
            {
                errMsg = "Type must be U or B or UB.\r\nU is reject Upper、B is reject Bottom、UB is reject both.";
                return Ok(errMsg);
            }
            var result = await _devTreatmentDAO.FindAll(x => x.SAMPLENO == sampleNo.Trim()).ToListAsync();
            if (result.Count > 0)
            {
                var biz_flag = result[0].BIZ_FLAG.Trim();
                if (biz_flag == "Y")
                {
                    var title = string.Format(@"麻煩協助刪除F340中介檔資料!");

                    var toMails = new List<string>();
                    toMails.Add("stan.chen@ssbshoes.com");
                    toMails.Add("aven.yu@ssbshoes.com");
                    toMails.Add("hsin.chen@ssbshoes.com");
                    var sqlDetail = string.Format(@"delete infxshcmes@ondbs:dev_f340 where spno in ( '{0}' )", sampleNo);
                    var sign = "\r\n\r\n\r\n陳尚賢Stan Chen\r\n--------------------------------------------------------------------------------------------------------------------\r\nInformation and Technology Center (資訊中心)-ERP\r\nSHYANG SHIN BAO industrial co., LTD (翔鑫堡工業股份有限公司)\r\nSHYANG HUNG CHENG CO.,LTD (翔鴻程責任有限公司)\r\nTel: +84 (0274)3745-001-025 #6696\r\nEmail : Stan.Chen@ssbshoes.com";
                    var content = string.Format(@"Dear Hsin:   請幫忙協助刪除中介檔資料，謝謝。 
                    {0}
                    
                    {1}", sqlDetail, sign);
                    await _sendMailService.SendListMailAsync(toMails, null, title, content, null);

                }
                result.ForEach(x =>
                    {
                        x.BIZ_FLAG = "N";
                        x.STATUS = "1";
                        x.BIZ_P_TIME = null;
                        if (type == "U")
                        {
                            x.DEV_DATE = null;
                            x.DEV_LOGIN = "";
                            x.TT_DATE = null;
                            x.TT_LOGIN = "";
                        }
                        else if (type == "B")
                        {
                            x.DEV_DATE2 = null;
                            x.DEV_LOGIN2 = "";
                            x.TT_DATE2 = null;
                            x.TT_LOGIN2 = "";
                        }
                        else if (type == "UB")
                        {
                            x.DEV_DATE = null;
                            x.DEV_LOGIN = "";
                            x.TT_DATE = null;
                            x.TT_LOGIN = "";
                            x.DEV_DATE2 = null;
                            x.DEV_LOGIN2 = "";
                            x.TT_DATE2 = null;
                            x.TT_LOGIN2 = "";
                        }
                        _devTreatmentDAO.Update(x);
                    }
                );

                await _devTreatmentDAO.SaveAll();
            }
            else
            {
                errMsg += String.Format(@"There is no data with this Sample No :{0}", sampleNo);
            }

            return Ok(errMsg);
        }
        //exportF340_ProcessPpd_pdf
        [HttpPost("exportF340_ProcessPpd_pdf")]
        public IActionResult exportF340_ProcessPpd_pdf(F340_PpdDto f340_ProcessDto)
        {

            _logger.LogInformation(String.Format(@"****** DKSController exportF340_ProcessPpd_pdf fired!! ******"));

            List<string> nastFileName = new List<string>();
            nastFileName.Add(f340_ProcessDto.DevSeason);
            nastFileName.Add(f340_ProcessDto.Article);
            nastFileName.Add(f340_ProcessDto.Pdf);
            var pathList = _fileService.GetLocalPath("F340PpdPic", nastFileName);
            byte[] result = System.IO.File.ReadAllBytes(pathList[1]);

            return File(result, "application/pdf");
        }
        [HttpPost("saveUBDate")]
        public async Task<IActionResult> SaveUBDate(F340_PpdDto dto)
        {

            _logger.LogInformation(String.Format(@"******DKSController SaveUBDate fired!! ******"));
            var partNo = dto.PartName.Split(" ")[0];
            var treatMentNo = dto.TreatMent.Split(" ")[0];
            DevTreatment model = _devTreatmentDAO.FindSingle(
                                                 x => x.SAMPLENO.Trim() == dto.SampleNo.Trim() &&
                                                 x.PARTNO.Trim() == partNo &&
                                                 x.TREATMENTCODE.Trim() == treatMentNo &&
                                                 x.FACTORYID == dto.Factory);
            if (model != null)
            {

                if (!String.IsNullOrEmpty(dto.CardDate)) model.U_REALCARD = dto.CardDate.ToDateTime();       //面部實務卡
                if (!String.IsNullOrEmpty(dto.ConfirmDate)) model.B_COLORCARD = dto.ConfirmDate.ToDateTime();   //底部色卡
                if (!String.IsNullOrEmpty(dto.ProcessDate)) model.WORKFLOW = dto.ProcessDate.ToDateTime();      //跨單位作業流程
                _devTreatmentDAO.Update(model);
                await _devTreatmentDAO.SaveAll();
            }
            return Ok();

        }

    }
}