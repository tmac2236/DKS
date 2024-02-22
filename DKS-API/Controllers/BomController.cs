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
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using Aspose.Cells;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DKS_API.Controllers
{
    public class BomController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly ISendMailService _sendMailService;
        private readonly IFileService _fileService;
        private readonly IDevBomFileDAO _devBomFileDAO;
        private readonly IDevBomStageDAO _devBomStageDAO;
        private readonly ISrfArtiBDAO _srfArtiBDAO;
        private readonly ISrfhDAO _srfhDAO;
        private readonly ISrfSizeBDAO _srfSizeBDAO;
        private readonly IDKSDAO _dksDAO;
        private readonly IArticledDAO _articledDAO;

        public BomController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger,
             IDevBomFileDAO devBomFileDAO, IDKSDAO dksDAO, IDevBomStageDAO devBomStageDAO, ISrfArtiBDAO srfArtiBDAO,
             ISrfhDAO srfhDAO, ISrfSizeBDAO srfSizeBDAO, IArticledDAO articledDAO,
             IExcelService excelService, ISendMailService sendMailService, IFileService fileService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _sendMailService = sendMailService;
            _fileService = fileService;
            _devBomFileDAO = devBomFileDAO;
            _devBomStageDAO = devBomStageDAO;
            _dksDAO = dksDAO;
            _srfArtiBDAO = srfArtiBDAO;
            _srfhDAO = srfhDAO;
            _srfSizeBDAO = srfSizeBDAO;
            _articledDAO = articledDAO;
        }
        [HttpGet("getDevBomFileDetailDto")]
        public async Task<IActionResult> GetDevBomFileDetailDto([FromQuery] SDevBomFile sDevBomFile)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevBomFile fired!! ******"));
            var data = new List<DevBomFileDetailDto>();
            if (sDevBomFile.UserTeam == "Y")
            {
                data = await _dksDAO.GetDevBomFileDto(sDevBomFile);
            }
            else if (sDevBomFile.UserTeam == "N")
            {
                data = await _dksDAO.GetDevBomFileNormalDto(sDevBomFile);
            }


            PagedList<DevBomFileDetailDto> result = PagedList<DevBomFileDetailDto>.Create(data, sDevBomFile.PageNumber, sDevBomFile.PageSize, sDevBomFile.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("addBOMfile")]
        public async Task<IActionResult> AddBOMfile([FromForm] UploadDevBomFileDto uploadDevBomFileDto)
        {

            _logger.LogInformation(String.Format(@"******BomController AddBOMfile fired!! ******"));

            List<string> articles = uploadDevBomFileDto.ArticleList.Replace(" ", "").Split(";").ToList();

            if (uploadDevBomFileDto.File.Length > 0 && articles.Count > 0)       //save to server
            {
                foreach (string oneArticle in articles)
                {
                    //Define Ver#
                    DevBomFile model = new DevBomFile();
                    //article list 的 article + stage最大ver# 20240106 Aven 確認
                    DevBomFile md = _devBomFileDAO.FindAll(
                                x => x.ARTICLE == oneArticle &&
                                x.FACTORY == uploadDevBomFileDto.FactoryId &&
                                x.STAGE == uploadDevBomFileDto.Stage).AsNoTracking().OrderByDescending(x => x.VER).FirstOrDefault();
                    if (md == null)
                    {
                        model.VER = 1;
                    }
                    else
                    {
                        model.VER = md.VER;
                        model.VER += 1;
                    }
                    // save file to server
                    var fileName = string.Format("{0}-{1}-V{2}.0-{3}-{4}-{5}.xlsx",
                    uploadDevBomFileDto.Season, uploadDevBomFileDto.Stage, model.VER, uploadDevBomFileDto.ModelName, uploadDevBomFileDto.ModelNo,
                    uploadDevBomFileDto.ArticleList.Replace(" ", ""));
                    List<string> nastFileName = new List<string>();
                    nastFileName.Add("ArticleBoms");
                    nastFileName.Add(uploadDevBomFileDto.Season);
                    nastFileName.Add(oneArticle);
                    nastFileName.Add(fileName);

                    if (await _fileService.SaveFiletoServer(uploadDevBomFileDto.File, "F340PpdPic", nastFileName))
                    {
                        _logger.LogInformation(String.Format(@"******BOMController AddBOMfile Add a xlsx: {0}!! ******", fileName));

                        //save to DAO (存N次)
                        model.FACTORY = uploadDevBomFileDto.FactoryId;
                        model.DEVTEAMID = uploadDevBomFileDto.Team;
                        model.SEASON = uploadDevBomFileDto.Season;
                        model.MODELNO = uploadDevBomFileDto.ModelNo;
                        model.MODELNAME = uploadDevBomFileDto.ModelName;
                        model.ARTICLE = oneArticle;
                        model.ARTICLE_LIST = uploadDevBomFileDto.ArticleList.Replace(" ", "");

                        model.STAGE = uploadDevBomFileDto.Stage;
                        model.FILENAME = fileName;
                        if (String.IsNullOrEmpty(uploadDevBomFileDto.Ecrno))
                        {
                            model.ECRNO = "";
                        }
                        else
                        {
                            model.ECRNO = uploadDevBomFileDto.Ecrno;
                        }

                        model.PDM_APPLY = "N";

                        //Sort
                        DevBomStage mds = _devBomStageDAO.FindSingle(
                                    x => x.FACTORY == uploadDevBomFileDto.FactoryId &&
                                    x.STAGE == uploadDevBomFileDto.Stage);
                        if (mds != null)
                        {
                            model.SORT = (short)(mds.SORT * 100);
                            model.SORT += model.VER;
                        }
                        if (String.IsNullOrEmpty(uploadDevBomFileDto.Remark))
                        {
                            model.REMARK = "";
                        }
                        else
                        {
                            model.REMARK = uploadDevBomFileDto.Remark.Trim();
                        }
                        model.APPLY = "N";
                        model.UPUSR = uploadDevBomFileDto.UpdateUser;
                        model.UPDAY = DateTime.Now;
                        _devBomFileDAO.Add(model);
                        await _devBomFileDAO.SaveAll();
                    }
                }


            }

            return Ok();
        }
        [HttpPost("applyBOMfile")]
        public async Task<IActionResult> ApplyBOMfile()
        {
            /*
             //Debug
            List<SendDevBomDetailMailListDto> mailss = await _dksDAO.GetSendDevBomDetailMailListDto("SMU","C","01");
            MailMessage mailTo = new MailMessage();
            foreach (SendDevBomDetailMailListDto item in mailss)
            {
                if (item.MailGroup.Contains(";"))
                {
                    var l = item.MailGroup.Split(";").ToList();
                    foreach (string i in l)
                    {
                        _logger.LogInformation(i);
                        mailTo.To.Add(i);
                    }
                }
                else
                {
                    _logger.LogInformation(item.MailGroup);
                    mailTo.To.Add(item.MailGroup);
                }

            }            
            //Debug 
            */
            _logger.LogInformation(String.Format(@"******BOMController ApplyBOMfile fired!! ******"));
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var ver = HttpContext.Request.Form["ver"].ToShort();
            var sort = HttpContext.Request.Form["sort"].ToShort();
            var remark = HttpContext.Request.Form["remark"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();

            var season = HttpContext.Request.Form["season"].ToString().Trim();
            var stage = HttpContext.Request.Form["stage"].ToString().Trim();
            var modelName = HttpContext.Request.Form["modelName"].ToString().Trim();
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            var articleList = HttpContext.Request.Form["articleList"].ToString().Trim();

            var devTeamId = HttpContext.Request.Form["devTeamId"].ToString().Trim();
            var factory = HttpContext.Request.Form["factory"].ToString().Trim();
            var devTeamName = "";
            if (factoryId.Contains(","))
            {
                factoryId = factoryId.Split(",")[0];
            }
            List<string> articles = articleList.Replace(" ", "").Split(";").ToList();
            // Team Name轉換 =>U廠的組別代號之後會是U1, U2, U3.....
            if (devTeamId == "01")
                devTeamName = "Team A";
            else if (devTeamId == "02")
                devTeamName = "Team B";
            else if (devTeamId == "03")
                devTeamName = "Team D";
            else if (devTeamId == "04")
                devTeamName = "Team E";
            else if (devTeamId == "05")
                devTeamName = "Team C";
            else if (devTeamId == "06")
                devTeamName = "Team 6";
            else if (devTeamId == "07")
                devTeamName = "Team 7";
            else if (devTeamId == "12")
                devTeamName = "Team 8";
            else
                devTeamName = "Unknown Team";
            /*2023/12/16 cancel override file
            IFormFile iFile = HttpContext.Request.Form.Files["file"];

            if (iFile != null)       //save to server
            {
                var fileName = string.Format("{0}-{1}-V{2}-{3}-{4}-{5}.xlsx", season, stage, ver, modelName, modelNo, article);

                // save file to server
                List<string> nastFileName = new List<string>();
                nastFileName.Add("ArticleBoms");
                nastFileName.Add(season);
                nastFileName.Add(article);
                nastFileName.Add(fileName);
                if (await _fileService.SaveFiletoServer(iFile, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******BOMController ApplyBOMfile Override a xlsx: {0}!! ******", fileName));
                }
            }
            */
            string fileName = "";
            foreach (string oneArticle in articles)
            {
                DevBomFile model = _devBomFileDAO.FindSingle(
                                    x => x.ARTICLE == oneArticle &&
                                    x.FACTORY == factoryId &&
                                    x.STAGE == stage &&
                                    x.SORT == sort);

                if (model != null)
                {
                    model.APPLY = "Y";
                    model.REMARK = remark;
                    model.UPDAY = DateTime.Now;
                    model.UPUSR = loginUser;
                    fileName = model.FILENAME;
                    _devBomFileDAO.Update(model);
                }

            }

            await _devBomFileDAO.SaveAll();

            var mailInformation = await _dksDAO.GetDevBomDetailMailDto(factoryId, article, stage, ver);
            if (mailInformation.Count == 0) return Ok();

            var subject = string.Format(@"{2}-{0}-{1}", "BOM", fileName.Replace(".xlsx", ""), devTeamName );
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD>");
            sb.Append("<style type=\"text/css\">");
            sb.Append(".OutBorder {");
            sb.Append("border: double;");
            sb.Append("}");
            sb.Append("</style>");
            sb.Append("</HEAD>");
            sb.Append("<body>");
            sb.Append("<table width='700' border='0' align='center' cellpadding='0' cellspacing='0' class='OutBorder' bgcolor='#CCCCCC'>");
            sb.Append("<tr bgcolor='#003366'><td colspan='5'><strong><font color='#FFFFFF' size='4'>" + "New Bom file has been applied" + "，Detail：</font></strong></td></tr>");

            sb.Append("<tr>");
            sb.Append("<td width='100%'>&nbsp;</td>");
            //sb.Append("<td width='55%'>&nbsp;</td>");
            //sb.Append("<td width='15%'>&nbsp;</td>");
            //sb.Append("<td width='25%'>&nbsp;</td>");
            //sb.Append("<td width='10%'>&nbsp;</td>");
            sb.Append("</tr>");
            //sb.Append(String.Format(@"<tr><td><strong>Bom Version/ Bom 版本:</strong></td><td colspan='4'>[{0}][{1}][{2}]</td></tr>", season, article, modelNo));
            Type dtoType = typeof(DevBomDetailMailDto);
            PropertyInfo[] properties = dtoType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //string propertyName = property.Name;
                object propertyValue = property.GetValue(mailInformation[0]);
                sb.Append(String.Format(@"<tr><td><strong>{0}</strong></td></tr>", propertyValue));
            }

            sb.Append("</tr>");
            sb.Append("<tr><td>&nbsp;</td></tr>"); //換行
            sb.Append("<tr><td colspan='5'><strong><font color='#600000' size='4'>This letter is sent automatically. Please do not reply directly.!!</td></tr>");
            sb.Append("<tr><td colspan='5'><strong><font color='#600000' size='4'>此郵件為系統自動發送，請勿回覆!!</td></tr>");
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</HTML>");
            var content = sb.ToString();


            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient(_config.GetSection("MailSettingServer:Server").Value);
            mail.From = new MailAddress(_config.GetSection("MailSettingServer:FromEmail").Value, _config.GetSection("MailSettingServer:FromName").Value);
            // Set the message body to HTML format
            mail.IsBodyHtml = true;
            List<SendDevBomDetailMailListDto> stageMails = await _dksDAO.GetSendDevBomDetailMailListDto(stage, factory, devTeamId);

            foreach (SendDevBomDetailMailListDto item in stageMails)
            {
                if (item.MailGroup.Contains(";"))
                {
                    var l = item.MailGroup.Split(";").ToList();
                    foreach (string i in l)
                    {
                        mail.To.Add(i);
                    }
                }
                else
                {
                    mail.To.Add(item.MailGroup);
                }

            }
            mail.Subject = subject;
            mail.Body = content;

            string rootdir = Directory.GetCurrentDirectory();
            var localStr = _config.GetSection("AppSettings:ArticleBomsRoot").Value;
            var path = rootdir + localStr;
            path = path.Replace("DKS-API", "DKS-SPA");
            string filePath = Path.Combine(path, season, article, fileName);
            _logger.LogInformation(String.Format(@"準備讀取檔案{0}", filePath));
            if (System.IO.File.Exists(filePath))
            {
                Attachment attachment = new Attachment(filePath);
                mail.Attachments.Add(attachment);
            }
            else
            {
                _logger.LogInformation(String.Format(@"檔案{0}，不存在，請Debug !!!!!!!", filePath));
            }

            smtpServer.Port = Convert.ToInt32(_config.GetSection("MailSettingServer:Port").Value);
            smtpServer.Credentials = new NetworkCredential(_config.GetSection("MailSettingServer:UserName").Value, _config.GetSection("MailSettingServer:Password").Value);
            smtpServer.EnableSsl = Convert.ToBoolean(_config.GetSection("MailSettingServer:EnableSsl").Value);

            await smtpServer.SendMailAsync(mail);
            _logger.LogInformation($"======================SendMailAsync() 成功!======================");

            return Ok();
        }
        [HttpGet("getDevTeamByLoginDto")]
        public async Task<IActionResult> GetDevTeamByLoginDto(string login)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevTeamByLoginDto fired!! ******"));
            var data = await _dksDAO.GetDevTeamByLoginDto(login);
            return Ok(data);
        }

        [HttpGet("checkHPSD138")]
        public async Task<IActionResult> CheckHPSD138(string article, string ecrNo)
        {
            _logger.LogInformation(String.Format(@"****** BOMController CheckHPSD138 fired!! ******"));
            var data = await _dksDAO.GetSsbGetHpSd138Dto(ecrNo);
            if (data.Count > 0)
            {
                List<string> rs = new List<string>();
                foreach (SsbGetHpSd138Dto d in data)
                {

                    string d1 = d.article.Replace("ART#", "");
                    if (!String.IsNullOrEmpty(d1)) d1 = d1.Trim();
                    if (d1.Contains("ALL")) return Ok(true);

                    string[] arts = d1.Split('/');
                    bool first = false;
                    string artTem = "";
                    foreach (string art in arts)
                    {
                        if (!first)
                        {
                            if (article == art) return Ok(true);
                            artTem = art;
                            first = true;
                        }
                        else
                        {
                            string artStr = artTem.Substring(0, artTem.Length - 2);
                            artStr += art;
                            if (article == artStr) return Ok(true);
                        }

                    }

                }

            }
            return Ok(false);
        }
        [HttpGet("getDevBomStage")]
        public async Task<IActionResult> GetDevBomStage()
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevBomStage fired!! ******"));
            var data = await _devBomStageDAO.FindAll().OrderBy(x => x.SORT).ToListAsync();
            return Ok(data);
        }
        [HttpPost("compareTwoExcel")]
        public IActionResult CompareTwoExcel()
        {
            _logger.LogInformation(String.Format(@"****** BomController CompareTwoExcel fired!! ******"));
            IFormFile bufferFile1 = HttpContext.Request.Form.Files["bufferFile1"];
            IFormFile bufferFile2 = HttpContext.Request.Form.Files["bufferFile2"];
            List<CellDifferenceDto> differences = new List<CellDifferenceDto>();
            byte[] result;
            using (var workbook1 = new Workbook(bufferFile1.OpenReadStream()))
            using (var workbook2 = new Workbook(bufferFile2.OpenReadStream()))
            using (var resultStream = new MemoryStream())
            {
                var worksheet1 = workbook1.Worksheets[0];
                var worksheet2 = workbook2.Worksheets[0];

                Cells cells1 = worksheet1.Cells;
                Cells cells2 = worksheet2.Cells;

                int maxRow = Math.Max(cells1.MaxDataRow + 1, cells2.MaxDataRow + 1);
                int maxColumn = Math.Max(cells1.MaxDataColumn + 1, cells2.MaxDataColumn + 1);

                for (int row = 0; row < maxRow; row++)
                {
                    for (int col = 0; col < maxColumn; col++)
                    {
                        var cell1 = cells1[row, col];
                        var cell2 = cells2[row, col];

                        if (cell1 != null && cell2 != null)
                        {
                            string value1 = cell1.StringValue;
                            string value2 = cell2.StringValue;

                            //if (cell1 != cell2)
                            if (value1 != value2)
                            {
                                differences.Add(new CellDifferenceDto
                                {
                                    CellName = CellsHelper.CellIndexToName(row, col),
                                    NewValue = cell1,
                                    OldValue = cell2
                                });
                            }
                        }
                    }
                }
                var newWSName = "Result";
                if (workbook1.Worksheets.Cast<Worksheet>().Any(sheet => sheet.Name == newWSName))
                {
                    workbook1.Worksheets.RemoveAt(newWSName);
                }
                int newWorksheetIndex = workbook1.Worksheets.Add(); // Add a new worksheet
                Worksheet newWorksheet = workbook1.Worksheets[newWorksheetIndex];
                newWorksheet.Name = newWSName;
                // Write column headers
                newWorksheet.Cells["A1"].PutValue("Cell");
                newWorksheet.Cells["B1"].PutValue("New Value");
                newWorksheet.Cells["C1"].PutValue("Old Value");

                newWorksheet.Cells.SetColumnWidthPixel(0, 40);
                newWorksheet.Cells.SetColumnWidthPixel(1, 500);
                newWorksheet.Cells.SetColumnWidthPixel(2, 500);

                // Write differences data
                for (int i = 0; i < differences.Count; i++)
                {
                    newWorksheet.Cells[$"A{i + 2}"].PutValue(differences[i].CellName);
                    newWorksheet.Cells[$"B{i + 2}"].PutValue(differences[i].NewValue.Value);
                    newWorksheet.Cells[$"B{i + 2}"].SetStyle(differences[i].NewValue.GetStyle());

                    newWorksheet.Cells[$"C{i + 2}"].PutValue(differences[i].OldValue.Value);
                    newWorksheet.Cells[$"C{i + 2}"].SetStyle(differences[i].OldValue.GetStyle());
                }

                newWorksheet.FreezePanes(1, 0, 1, 2); //frozen A1:C1
                workbook1.Save(resultStream, SaveFormat.Xlsx);

                // Convert the result stream to a byte array
                result = resultStream.ToArray();

            }
            return File(result, "application/xlsx");

        }
        [HttpGet("getSrfArticleDto")]
        public IActionResult GetSrfArticleDto(string srfId)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetSrfArticleDto fired!! ******"));
            var d = _srfhDAO.GetSrfArticleDto(srfId);
            return Ok(d);
        }
        [HttpPost("copySrf")]
        public async Task<IActionResult> CopySrf()
        {
            _logger.LogInformation(String.Format(@"******BomController CopySrf fired!! ******"));
            var f1 = HttpContext.Request.Form["f1"].ToString();
            var f2 = HttpContext.Request.Form["f2"].ToString();

            var j1 = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(f1);
            var j2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SrfArticleDto>>(f2);
            string srfIdF = j1.Value<string>("srfIdF");
            string srfIdT = j1.Value<string>("srfIdT");
            string stageT = j1.Value<string>("stageT");

            Srfh check = _srfhDAO.FindSingle(x => x.SRFID == srfIdT);
            if (check != null) return Ok("SRFID is duplicate in System, please change a new Id.");
            Srfh dbF = _srfhDAO.FindSingle(x => x.SRFID == srfIdF);
            if (dbF != null)
            {
                string jsonStr = JsonSerializer.Serialize(dbF);
                Srfh toAdd = JsonSerializer.Deserialize<Srfh>(jsonStr);

                string last = _srfhDAO.FindAll().OrderByDescending(x => x.PKSRFBID).Take(1).Select(x => x.PKSRFBID).ToList().FirstOrDefault();
                int number = last.Replace("CAH", "").ToInt();
                number += 1;
                toAdd.PKSRFBID = String.Format("{0:CAH000000000}", number);

                toAdd.SRFID = srfIdT;
                toAdd.SAMPURSRF = stageT;
                toAdd.INSERDATE = DateTime.Now;
                toAdd.MDUSERID = 0;
                toAdd.CHANGDATE = null;
                _srfhDAO.Add(toAdd);

                List<SrfSizeB> sizes = _srfSizeBDAO.FindAll(x => x.SRFID == srfIdF).AsNoTracking().ToList();
                foreach (SrfSizeB s in sizes)
                {
                    s.SRFID = srfIdT;
                    _srfSizeBDAO.Add(s);
                }
                List<SrfArtiB> artiles = _srfArtiBDAO.FindAll(x => x.SRFID == srfIdF).AsNoTracking().ToList();
                foreach (SrfArtiB a in artiles)
                {
                    a.SRFID = srfIdT;
                    _srfArtiBDAO.Add(a);
                }
            }
            await _srfhDAO.SaveAll();
            return Ok();
        }
        [HttpPost("returnBom")]
        public async Task<IActionResult> ReturnBom()
        {

            _logger.LogInformation(String.Format(@"******BOMController ReturnBom fired!! ******"));
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            //須改 article List
            var articleList = HttpContext.Request.Form["articleList"].ToString().Trim();
            var sort = HttpContext.Request.Form["sort"].ToShort();

            var stage = HttpContext.Request.Form["stage"].ToString().Trim();
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            var fileName = HttpContext.Request.Form["fileName"].ToString().Trim();
            string[] parts = fileName.Split('-');
            List<string> articles = articleList.Replace(" ", "").Split(";").ToList();
            foreach (string oneArticle in articles)
            {
                List<string> nastFileName = new List<string>();
                nastFileName.Add("ArticleBoms");
                nastFileName.Add(parts[0]);                     //season
                nastFileName.Add(oneArticle);                   //article
                nastFileName.Add(fileName);                     //fileName

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    DevBomFile model = _devBomFileDAO.FindSingle(
                                        x => x.ARTICLE == oneArticle &&
                                        x.FACTORY == factoryId &&
                                        x.STAGE == stage &&
                                        x.SORT == sort);
                    _devBomFileDAO.Remove(model);
                    await _devBomFileDAO.SaveAll();
                }

            }




            return Ok();
        }
        [HttpPost("getArticleList")]
        public async Task<IActionResult> GetArticleList()
        {

            _logger.LogInformation(String.Format(@"******BOMController GetArticleList fired!! ******"));
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();

            IFormFile iFile = HttpContext.Request.Form.Files["file"];
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            string cellValue = "";

            using (Stream stream = iFile.OpenReadStream())
            {
                Workbook workbook = new Workbook(stream);
                Worksheet worksheet = workbook.Worksheets[0];
                Cell cell = worksheet.Cells["B4"];
                cellValue = cell.StringValue;
            }
            List<string> aryExcel = cellValue.Replace(" ", "").Split(';').OrderBy(s => s).ToList();
            aryExcel.RemoveAll(s => s == "");
            List<string> dbArticleList = await _articledDAO.GetArticleListByModelNo(factoryId, modelNo);
            if (!aryExcel.Contains(article)) return Ok("Error: The Article didn't fit Article List in this Excel!");
            /* 取消F205 Article全部符合才能傳的卡控
            //dbArticleList(主) 需 >= aryExcel                        
            var isOk = dbArticleList.All( db => aryExcel.Contains(db));
            if(!isOk){
                var mis = dbArticleList.Except(aryExcel).ToList();
                return Ok(string.Format(@"Error:The file miss article:{0}  ,F205={1}  ,excel={2}"
                            ,string.Join(";", mis) , string.Join(";", dbArticleList) , string.Join(";", aryExcel)));
            }else{
            */
            var surplus = aryExcel.Except(dbArticleList).ToList();
            if (surplus.Count == 0)
            {
                return Ok(cellValue);
            }
            else
            {
                //可上傳但須警示
                return Ok(string.Format(@"Alert,{0},{1}"
                            , string.Join(";", surplus), string.Join(";", dbArticleList)));
            }

            //} 
        }

    }

}
