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

namespace DKS_API.Controllers
{
    public class BomController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly ISendMailService _sendMailService;
        private readonly IFileService _fileService;
        private readonly IDevBomFileDAO _devBomFileDAO;
        private readonly IDKSDAO _dksDAO;

        public BomController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger,
             IDevBomFileDAO devBomFileDAO,IDKSDAO dksDAO,
             IExcelService excelService, ISendMailService sendMailService, IFileService fileService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _sendMailService = sendMailService;
            _fileService = fileService;
            _devBomFileDAO = devBomFileDAO;
            _dksDAO = dksDAO;

        }
        [HttpGet("getDevBomFileDetailDto")]
        public async Task<IActionResult> GetDevBomFileDetailDto([FromQuery] SDevBomFile sDevBomFile)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevBomFile fired!! ******"));
            var data =  await _dksDAO.GetDevBomFileDto(sDevBomFile);

            PagedList<DevBomFileDetailDto> result = PagedList<DevBomFileDetailDto>.Create(data, sDevBomFile.PageNumber, sDevBomFile.PageSize, sDevBomFile.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("addBOMfile")]
        public async Task<IActionResult> AddBOMfile([FromForm] UploadDevBomFileDto uploadDevBomFileDto)
        {

            _logger.LogInformation(String.Format(@"******BomController AddBOMfile fired!! ******"));


            // FactoryId + Season + Article + Id .pdf
            var fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}.xlsx", uploadDevBomFileDto.Season,uploadDevBomFileDto.Stage, uploadDevBomFileDto.Ver, uploadDevBomFileDto.ModelName,uploadDevBomFileDto.ModelNo,uploadDevBomFileDto.Article);

            // save file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("ArticleBoms");
            nastFileName.Add(uploadDevBomFileDto.Season);
            nastFileName.Add(uploadDevBomFileDto.Article);
            nastFileName.Add(fileName);

            DevBomFile model = new DevBomFile();
            if (uploadDevBomFileDto.File.Length > 0)       //save to server
            {
                if (await _fileService.SaveFiletoServer(uploadDevBomFileDto.File, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******BOMController AddBOMfile Add a xlsx: {0}!! ******", fileName));
                    //save to DAO
                    model.FACTORY = uploadDevBomFileDto.FactoryId;
                    model.DEVTEAMID = uploadDevBomFileDto.Team;
                    model.SEASON = uploadDevBomFileDto.Season;                    
                    model.MODELNO = uploadDevBomFileDto.ModelNo;
                    model.MODELNAME = uploadDevBomFileDto.ModelName;
                    model.ARTICLE = uploadDevBomFileDto.Article;

                    model.STAGE = uploadDevBomFileDto.Stage;
                    model.VER = uploadDevBomFileDto.Ver.ToShort();
                    model.FILENAME = fileName;

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

            return Ok(model);
        }    
        [HttpPost("applyBOMfile")]
        public async Task<IActionResult> ApplyBOMfile()
        {

            _logger.LogInformation(String.Format(@"******DTRController ApplyBOMfile fired!! ******"));
            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var ver = HttpContext.Request.Form["ver"].ToShort();
            var remark = HttpContext.Request.Form["remark"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();

            var season = HttpContext.Request.Form["season"].ToString().Trim();
            var stage = HttpContext.Request.Form["stage"].ToString().Trim();
            var modelName = HttpContext.Request.Form["modelName"].ToString().Trim();
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            IFormFile iFile = HttpContext.Request.Form.Files["file"];

            if (iFile != null)       //save to server
            {
                var fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}.xlsx", season,stage, ver, modelName,modelNo,article);

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


            DevBomFile model = _devBomFileDAO.FindSingle(
                                 x => x.ARTICLE == article &&
                                 x.FACTORY == factoryId &&
                                 x.VER == ver );
            model.APPLY = "Y";
            model.REMARK = remark;
            model.UPDAY = DateTime.Now;
            model.UPUSR = loginUser;

            _devBomFileDAO.Update(model);
            await _devBomFileDAO.SaveAll();

            var toMails = new List<string>();
            toMails.Add("stan.chen@ssbshoes.com");

            var subject = $"New BOM file Apply";

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
                sb.Append("<tr bgcolor='#003366'><td colspan='5'><strong><font color='#FFFFFF' size='4'>" + "New Bom file was apply" + "，Detail：</font></strong></td></tr>");

                sb.Append("<tr>");
                sb.Append("<td width='35%'>&nbsp;</td>");
                sb.Append("<td width='55%'>&nbsp;</td>");
                //sb.Append("<td width='15%'>&nbsp;</td>");
                //sb.Append("<td width='25%'>&nbsp;</td>");
                //sb.Append("<td width='10%'>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append(String.Format(@"<tr><td><strong>Bom Version/ Bom 版本:</strong></td><td colspan='4'>[{0}][{1}][{2}]</td></tr>", season,article,modelNo));
                //sb.Append(String.Format(@"<tr><td><strong>Bom Version/ Bom 版本:</strong></td><td colspan='4'>{0}{1}</td></tr>", stage,ver));
                //sb.Append("<tr><td><strong>Bom Data/ Bom 資訊:</strong></td><td colspan='12'>" + "[SS24][NJQ56][#IF3509]" + "</td></tr>");


                sb.Append("</tr>");
                sb.Append("<tr><td>&nbsp;</td></tr>"); //換行

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
                foreach (var item in toMails)
                {
                    mail.To.Add(item);
                }
                mail.Subject = subject;
                mail.Body = content;

                //pending夾帶檔案
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(new MemoryStream(result), "EmailDetailByBatch" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                //mail.Attachments.Add(attachment);

                smtpServer.Port = Convert.ToInt32(_config.GetSection("MailSettingServer:Port").Value);
                smtpServer.Credentials = new NetworkCredential(_config.GetSection("MailSettingServer:UserName").Value, _config.GetSection("MailSettingServer:Password").Value);
                smtpServer.EnableSsl = Convert.ToBoolean(_config.GetSection("MailSettingServer:EnableSsl").Value);

                await smtpServer.SendMailAsync(mail);
                 _logger.LogInformation($"======================SendMailAsync() 成功!======================");

            return Ok(model);
        }
        [HttpGet("getDevTeamByLoginDto")]
        public async Task<IActionResult> GetDevTeamByLoginDto(string login)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevTeamByLoginDto fired!! ******"));
            var data =  await _dksDAO.GetDevTeamByLoginDto(login);
            return Ok(data);
        }

    }

}
