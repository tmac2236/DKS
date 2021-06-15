using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Services.Implement
{
    public class SendMailService : ISendMailService
    {
        private readonly IConfiguration _config;
        private ILogger<SendMailService> _logger;
        public SendMailService(IConfiguration config, ILogger<SendMailService> logger)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}