using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using DKS_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Services.Implement
{
    public class F340CheckService : IF340CheckService
    {
        private readonly IConfiguration _config;
        private readonly IDevTreatmentDAO _devTreatmentDAO;
        private ILogger<F340CheckService> _logger;
        public F340CheckService(IConfiguration config, ILogger<F340CheckService> logger
            , IDevTreatmentDAO devTreatmentDAO)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _devTreatmentDAO = devTreatmentDAO;
        }

        public async Task<string> IE340CheckMain()
        {
 
            string checkString = "";
            var toDoList = await CheckToDoF340List();
            if(toDoList.Count > 0){
                //step 2 : 太複雜先pending
                //await CheckPDM();
                await CheckF340TT();
            }

            return checkString;
        }
        //Step1 :檢查TT有沒有要放行的資料==>PM5:00檢查今天有沒有要拋轉的資料(一切的原頭)
        public async Task<List<string>> CheckToDoF340List()
        {
            var list = await _devTreatmentDAO.FindAll(x => x.TT_LOGIN != "" && x.TT_LOGIN2 != ""
                           && x.RELEASE_LOGIN == "" && x.BIZ_FLAG == "N").Select(x => new
                           {
                               SAMPLENO = x.SAMPLENO
                           }).Distinct().ToListAsync();
            return list.Select(x => x.SAMPLENO).ToList();
        }
        //STEP2 :檢查PDM已放行, 但HP部位是空值==>Dev在一個部位有兩個加工的情況，補上的情況(在巨集做)=>幫忙補上另一個空的HP部位
        public async Task<bool> CheckPDM()
        {
            bool isSuccess = false;
            var list2 = await _devTreatmentDAO.FindAll( x => x.PDM_LOGIN !="" && x.HPPARTNO =="" && x.BIZ_FLAG =="N" ).Select(x => x.SAMPLENO).ToListAsync();
            
                          
            return isSuccess;
        }

        //STEP3檢查TT
        public async Task<string> CheckF340TT()
        {
            string checkString = "";
            //STEP3.1 :檢查TT已放行,有廠商沒加工
            var list3_1 = await _devTreatmentDAO.FindAll(x => x.SUPCODE != "" && x.TREATMENTCODE == "").ToListAsync();
            list3_1.ForEach(x =>{
                x.SUPCODE = "";
                x.TREATMENTCODE = "";
                _devTreatmentDAO.Update(x);
            });
            //STEP 3.2 :檢查TT已放行,檢查有工段但沒加工項目
            var list3_2 = await _devTreatmentDAO.FindAll(x => x.BIZ_FLAG == "N" && x.TREATMENTCODE == "" 
                            && x.TT_LOGIN != "" && x.TT_LOGIN2 != "" && (x.WORKSHP != "" || x.CATEGORY != "") ).ToListAsync();
            list3_2.ForEach(x => {
                x.WORKSHP = "";
                x.CATEGORY = "";
                x.HPSUPID = "";
                x.SUPCODE = "";  
                _devTreatmentDAO.Update(x);             
            });            
            //STEP3.3 :檢查TT已放行, 有加工項目但沒HP廠商==>寄信給開發Bernie助理
            var list3_3 = await _devTreatmentDAO.FindAll(x => x.TREATMENTCODE != "" && x.HPSUPID == ""
                           && x.BIZ_FLAG == "N" && x.TT_LOGIN != "" && x.TT_LOGIN2 != "").Select( x =>x.SAMPLENO).ToListAsync();
            //Sent Mail............
            MailMessage mail = new MailMessage();
            var test = _config.GetSection("MailSettingServer:Server").Value;
            SmtpClient smtpServer = new SmtpClient(_config.GetSection("MailSettingServer:Server").Value);
            mail.From = new MailAddress(_config.GetSection("MailSettingServer:FromEmail").Value, _config.GetSection("MailSettingServer:FromName").Value);


            mail.To.Add("hongluong@shc.ssbshoes.com");
            mail.To.Add("thoi.dang@shc.ssbshoes.com");
            mail.To.Add("stan.chen@ssbshoes.com");
            mail.To.Add("aven.yu@ssbshoes.com");

            mail.Subject = "Check F340 Supplier Code";
            mail.Body = String.Format(@"Please check these sampleNo and update Supplier code : {0}", String.Join(",",list3_3));

            smtpServer.Port = Convert.ToInt32(_config.GetSection("MailSettingServer:Port").Value);
            smtpServer.Credentials = new NetworkCredential(_config.GetSection("MailSettingServer:UserName").Value, _config.GetSection("MailSettingServer:Password").Value);
            smtpServer.EnableSsl = Convert.ToBoolean(_config.GetSection("MailSettingServer:EnableSsl").Value);

            await smtpServer.SendMailAsync(mail);           
            //STEP3.4 :檢查TT已放行, 有加工項目但無工段及類別資料==>清空TT_LOGIN、TT_Date技轉面部(HpPartNo != 2016)或底部(HpPartNo = 2016)重作
            var list3_4 = await _devTreatmentDAO.FindAll( x => x.BIZ_FLAG == "N" && x.TREATMENTCODE != "" &&
                             x.TT_LOGIN != "" && x.TT_LOGIN2 != "" && (x.WORKSHP == "" || x.CATEGORY == "") ).ToListAsync();
            list3_4.ForEach( x =>{
                if(x.HPPARTNO != "2016"){   //upper
                    x.TT_LOGIN = "";
                    x.TT_DATE = null;
                }else{                      //bottom
                    x.TT_LOGIN2 = "";
                    x.TT_DATE2 = null;
                }
                _devTreatmentDAO.Update(x);  
            });

            return checkString;
        }

    }
}