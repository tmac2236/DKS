using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aspose.Cells;
using DKS_API.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        //constructor

        public ApiController(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }
        protected byte[] CommonExportReport(object data, string templateName)
        {

            string rootStr = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            return stream.ToArray(); ;
        }
        protected byte[] CommonExportReportTabs(List<object> dataList, string templateName)
        {

            string rootStr = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            int index = 0;
            foreach (object data in dataList)
            {
                Worksheet ws = designer.Workbook.Worksheets[index];
                designer.SetDataSource("result", data);
                index++;
            }
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            return stream.ToArray(); ;
        }
        //儲存檔案到Server,If file is null resent to do Delete
        //file:檔案 
        //settingNam: root資料夾名稱
        //fileNames: 檔案名稱(含分層路徑)
        protected async Task<Boolean> SaveFiletoServer(IFormFile file, string settingNam, List<string> fileNames)
        {
            Boolean isSuccess = false;
            try
            {
                string rootdir = Directory.GetCurrentDirectory();
                var localStr = _config.GetSection("AppSettings:" + settingNam).Value;
                var pjName = _config.GetSection("AppSettings:ProjectName").Value;

                string innerPath = "";
                string fileName = string.Join("\\",fileNames.GetRange( fileNames.Count-1 , 1 ));
                //folder path
                if( fileNames.Count > 1 ){
                    innerPath = string.Join("\\",fileNames.GetRange( 0, fileNames.Count-1 ));
                }

                var pathToSave = rootdir + localStr + innerPath;    //新增資料夾的全路徑
                pathToSave = pathToSave.Replace(pjName + "-API", pjName + "-SPA");
                var fullPath = pathToSave + "\\" + fileName;   //新增檔名的全路徑
                if (file != null)
                {

                    if (!Directory.Exists(pathToSave))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathToSave);
                    }
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    isSuccess = true;
                }
                else
                {   //upload null present Delete
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return isSuccess;
            }

            return isSuccess;
        }
        //寄信
        //toMail: 收件人陣列 
        //subject: 郵件主題
        //content: 郵件內容
        //filePath: 郵件附件
        public async Task SendListMailAsync(List<string> toMail, string subject, string content, string? filePath)
        {
            MailMessage mail = new MailMessage();
            var test = _config.GetSection("MailSettingServer:Server").Value;
            SmtpClient smtpServer = new SmtpClient(_config.GetSection("MailSettingServer:Server").Value);
            mail.From = new MailAddress(_config.GetSection("MailSettingServer:FromEmail").Value, _config.GetSection("MailSettingServer:FromName").Value);

            foreach (var item in toMail)
            {
                mail.To.Add(item);
            }
            mail.Subject = subject;
            mail.Body = content;
            if (filePath != null)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(filePath);
                mail.Attachments.Add(attachment);
            }

            smtpServer.Port = Convert.ToInt32(_config.GetSection("MailSettingServer:Port").Value);
            smtpServer.Credentials = new NetworkCredential(_config.GetSection("MailSettingServer:UserName").Value, _config.GetSection("MailSettingServer:Password").Value);
            smtpServer.EnableSsl = Convert.ToBoolean(_config.GetSection("MailSettingServer:EnableSsl").Value);

            try
            {
                await smtpServer.SendMailAsync(mail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}