using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DKS_API.Services.Implement
{
    public class SendMailService : ISendMailService
    {
        private readonly IDKSDAO _dksDao;
        private readonly IConfiguration _config;
        private ILogger<SendMailService> _logger;
        public SendMailService(IDKSDAO dksDao,IConfiguration config, ILogger<SendMailService> logger)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dksDao = dksDao;
        }
        //寄信 ver1.
        //toMail: 收件人陣列
        //toCCMail: CC收件人陣列  
        //subject: 郵件主題
        //content: 郵件內容
        //filePath: 郵件附件
        public async Task SendListMailAsync(List<string> toMail, List<string>? toCCMail, string subject, string content, string? filePath)
        {
            try
            {
                MailMessage mail = new MailMessage();
                var test = _config.GetSection("MailSettingServer:Server").Value;
                SmtpClient smtpServer = new SmtpClient(_config.GetSection("MailSettingServer:Server").Value);
                mail.From = new MailAddress(_config.GetSection("MailSettingServer:FromEmail").Value, _config.GetSection("MailSettingServer:FromName").Value);

                foreach (var item in toMail)
                {
                    mail.To.Add(item);
                }
                if (toCCMail != null)
                {
                    foreach (var cc in toCCMail)
                    {
                        mail.Bcc.Add(cc);
                    }
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


                await smtpServer.SendMailAsync(mail);

            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!SendListMailAsync have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }
        //寄信 by byteArray
        //toMail: 收件人陣列
        //toCCMail: CC收件人陣列  
        //subject: 郵件主題
        //content: 郵件內容
        //filePath: 郵件附件 byte Array
        public async Task SendListMailAsyncbyByte(List<string> toMail, List<string>? toCCMail, string subject, string content, byte[] file)
        {
            try
            {
                MailMessage mail = new MailMessage();
                var test = _config.GetSection("MailSettingServer:Server").Value;
                SmtpClient smtpServer = new SmtpClient(_config.GetSection("MailSettingServer:Server").Value);
                mail.From = new MailAddress(_config.GetSection("MailSettingServer:FromEmail").Value, _config.GetSection("MailSettingServer:FromName").Value);

                foreach (var item in toMail)
                {
                    mail.To.Add(item);
                }
                if (toCCMail != null)
                {
                    foreach (var cc in toCCMail)
                    {
                        mail.Bcc.Add(cc);
                    }
                }
                mail.Subject = subject;
                mail.Body = content;


                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(new MemoryStream(file), "SampleTrackReport.xlsx");
                mail.Attachments.Add(attachment);

                smtpServer.Port = Convert.ToInt32(_config.GetSection("MailSettingServer:Port").Value);
                smtpServer.Credentials = new NetworkCredential(_config.GetSection("MailSettingServer:UserName").Value, _config.GetSection("MailSettingServer:Password").Value);
                smtpServer.EnableSsl = Convert.ToBoolean(_config.GetSection("MailSettingServer:EnableSsl").Value);


                await smtpServer.SendMailAsync(mail);

            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!SendListMailAsync have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        public async Task<bool> SendRFIDAlert(){
            
            _logger.LogInformation(String.Format(@"****** SendMailService SendRFIDAlert start!! ******"));

            var nowTime = DateTime.Now;

            var starTime = new TimeSpan(7, 30, 0);  //07:30 上班  //07:30 上班
            var endTime = new TimeSpan(16, 30, 0);  //16:30 上班
            if( !(starTime<nowTime.TimeOfDay&& nowTime.TimeOfDay<endTime) ){
                _logger.LogInformation(String.Format(@"****** SendMailService Working Time No need send !!! ******"));
                return false;
            } 

            var data = await _dksDao.GetPrdRfidAlertDto(""); //撈現在前三分鐘內

            _logger.LogInformation(String.Format(@"{0}: Today DTO count: {1}", nowTime.ToString(),data.Count.ToString() ));
            // data = data.FindAll(x=>x.Time >= nowTime.AddMinutes(-3));
            _logger.LogInformation(String.Format(@"recent 3 minutes DTO count: {0}", data.Count.ToString() ));
            if(data.Count <=0 ) {
                 _logger.LogInformation(String.Format(@"****** SendMailService No RFID in this trigger !!! ******"));
                 data = null;
                return false;
            }

            string result = "Please Check below RFID Area !!! ( ex. Gate[Time] )";
            foreach(PrdRfidAlertDto i in data){

                result += String.Format(@"   {0} ( {1} )", i.Gate,i.Time );

            }
            data = null;
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                
                 var localStr = _config.GetSection("AppSettings:RFIDApiUrl").Value;
                 var str = "payload={\"text\": \"" + result + "\"}";
                 HttpResponseMessage  res = client.PostAsync(localStr, new StringContent(str)).Result;
                 return true;
                 
            }
        }
    }
}