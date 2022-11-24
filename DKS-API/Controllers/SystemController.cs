using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Drawing;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DKS_API.DTOs;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using DKS_API.Helpers;
using DKS_API.Services.Interface;
using Newtonsoft.Json.Linq;

namespace DKS_API.Controllers
{
    public class SystemController : ApiController
    {
        private readonly IDevSysSetDAO _devSysSetDAO;
        private readonly IDKSDAO _dksDAO;
        private readonly IDevGateLogDataLogDAO _devGateLogDataLogDAO;
        private readonly IExcelService _excelService;

        public SystemController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<SystemController> logger,
                 IDevSysSetDAO devSysSetDAO,IDKSDAO dksDAO, IDevGateLogDataLogDAO devGateLogDataLogDAO
                 , IExcelService excelService)
                : base(config, webHostEnvironment, logger)
        {
            _devSysSetDAO = devSysSetDAO;
            _dksDAO = dksDAO;
            _devGateLogDataLogDAO = devGateLogDataLogDAO;
            _excelService = excelService;
        }

        [HttpGet("findAll")]
        public async Task<IActionResult> FindAll()
        {
            _logger.LogInformation(String.Format(@"****** SystemController FindAll fired!! ******"));

            var result = await _devSysSetDAO.GetAll().ToListAsync();
            return Ok(result);

        }
        [HttpPost("eidtSysSet")]
        public async Task<IActionResult> EidtSysSet(DevSysSet devSysSet)
        {
            _logger.LogInformation(String.Format(@"****** SystemController EidtSysSet fired!! ******"));

            string errorStr = "";

            DevSysSet opRecord = _devSysSetDAO.FindSingle(
            x => x.SYSKEY.Trim() == devSysSet.SYSKEY.Trim());


            opRecord.SYSVAL = devSysSet.SYSVAL.Trim();
            opRecord.UPUSR = devSysSet.UPUSR.Trim();
            opRecord.UPTIME = DateTime.Now;
            _devSysSetDAO.Update(opRecord);
            await _devSysSetDAO.SaveAll();
            return Ok(errorStr);

        }
        [HttpGet("getKanbanDataByLineDto")]
        public async Task<IActionResult> GetKanbanDataByLineDto(string lineId)
        {
            _logger.LogInformation(String.Format(@"****** SystemController GetKanbanDataByLineDto fired!! ******"));
            var data = await _dksDAO.GetKanbanDataByLineDto(lineId);
            return Ok(data);
        }           
        [HttpGet("getKanbanTQCDto")]
        public async Task<IActionResult> GetKanbanTQCDto(string lineId)
        {
            _logger.LogInformation(String.Format(@"****** SystemController GetKanbanTQCDto fired!! ******"));
            var data = await _dksDAO.GetKanbanTQCDto(lineId);
            return Ok(data);
        }        
        [HttpPost("sendSynoBot")]
        public  IActionResult SendSynoBot([FromForm] SynoBotDto sysnoDto)
        {
            _logger.LogInformation(String.Format(@"****** SystemController SendSynoBot fired!! ******"));
            SynoBotDto rep = new SynoBotDto();
            var localStr = _config.GetSection("AppSettings:SopUrl").Value;
            string[] switchStrings = {"F340","F432"};

                switch (switchStrings.FirstOrDefault<string>(s => sysnoDto.Text.ToUpper().Contains(s)))
                {
                    case "F432": 
                        string fileName = "F432Edit.pdf";
                        rep.Text = String.Format(@"Please follow the SOP!! <{0}{1}|Click Me!!!>", localStr, fileName );
                        break;

                    default:
                        {
                        rep.Text = "Sorry, Dobby have no idea, Please ask Aven.";
                            break;
                        }
                }
  
            return Ok(rep);
        } 
        [HttpGet("sendRfidAlert")]
        public IActionResult SendRfidAlert(string message)
        {
            _logger.LogInformation(String.Format(@"****** SystemController SendRfidAlert fired!! ******"));
            var dd = new DateTime();
            var localStr = _config.GetSection("AppSettings:RFIDApiUrl").Value;
            var nowTime = DateTime.Now.AddHours(-3);
            var starTime = new DateTime(1911, 10, 10, 7,30,0).TimeOfDay ;  //07:30 上班
            var endTime = new DateTime(1911, 10, 10, 16,30,0).TimeOfDay ;  //16:30 上班
            /*上班測試中
            if( starTime<nowTime.TimeOfDay&& nowTime.TimeOfDay<endTime ){
                return Ok("Working Time No need send!");
            } 
            */
            /*
            string recordTime = nowTime.ToString("yyyy-MM-dd HH:mm:ss");
            var data = await _dksDAO.GetPrdEntryAccessDto(area,recordTime);
            if(data.Count <=0 ) return Ok("No Warn Record!");

            foreach(PrdEntryAccessDto i in data){
                result += "   ";
                result += String.Format(@"{0}[{1}]", i.Gate.Trim(),i.BarcodeNo.Trim() );
            }
            */

            string result = "Please Check below RFID Area !!! ( ex. Gate[Barcode] )";
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using (var client = new HttpClient(httpClientHandler))
            {

                 var str = "payload={\"text\": \"" + result + message + "\"}";
                 HttpResponseMessage  res = client.PostAsync(localStr, new StringContent(str)).Result;
                 return Ok(res);

            }
            
        } 
        [HttpGet("getRfidAlert")]
        public async Task<IActionResult> GetRfidAlert([FromQuery]SRfidMaintain sRfidMaintain)
        {
            _logger.LogInformation(String.Format(@"****** SystemController GetRfidAlert fired!! ******"));
            DateTime dtS = DateTime.Parse(sRfidMaintain.recordTimeS);
            DateTime dtE = DateTime.Parse(sRfidMaintain.recordTimeE);
            var data = await _dksDAO.GetPrdRfidAlertDto(dtS.ToString("yyyy-MM-dd HH:mm:ss"),dtE.ToString("yyyy-MM-dd HH:mm:ss")); 

            PagedList<PrdRfidAlertDto> result = PagedList<PrdRfidAlertDto>.Create(data, sRfidMaintain.PageNumber, sRfidMaintain.PageSize, sRfidMaintain.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);

            return Ok(result);
        }
        [HttpPost("exportRfidAlert")]
        public  async Task<IActionResult>  ExportRfidAlert(SRfidMaintain sRfidMaintain)
        {
            _logger.LogInformation(String.Format(@"****** SystemController exportRfidAlert fired!! ******"));

            DateTime dtS = DateTime.Parse(sRfidMaintain.recordTimeS);
            DateTime dtE = DateTime.Parse(sRfidMaintain.recordTimeE);
            var data = await _dksDAO.GetPrdRfidAlertDto(dtS.ToString("yyyy-MM-dd HH:mm:ss"),dtE.ToString("yyyy-MM-dd HH:mm:ss")); 

            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempRfidMaintain.xlsx");

            return File(result, "application/xlsx");
        }        
        [HttpPost("setRfidAlert/{reason}/{updater}")]
        public async Task<IActionResult> SetRfidAlert(List<PrdRfidAlertDto> prdRfidAlertDtos, string reason,string updater)
        {
            _logger.LogInformation(String.Format(@"****** SystemController SetRfidAlert fired!! ******"));

            foreach(PrdRfidAlertDto item in prdRfidAlertDtos){
                var model =  _devGateLogDataLogDAO.FindSingle(x=>x.SEQ == Convert.ToInt32(item.Seq));
                if( model == null){
                    model = new DevGateLogDataLog();
                    model.SEQ = Convert.ToInt32(item.Seq);
                    model.REASON  =  reason;
                    model.UPDATER =  updater;
                    model.UPDATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    _devGateLogDataLogDAO.Add(model);
                }else{
                    //只有上一個修改者才能覆蓋自己的那一筆
                    if(item.Updater == updater){    
                        model.REASON  =  reason;
                        model.UPDATER =  updater;
                        model.UPDATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        _devGateLogDataLogDAO.Update(model);
                    }
                }  
            }
            await _devGateLogDataLogDAO.SaveAll();
            return Ok();
        }
        [HttpGet("loginRuRu")]
        public async Task<IActionResult> LoginRuRu(string account, string password)
        {
            _logger.LogInformation(String.Format(@"****** SystemController LoginRuRu fired!! ******"));

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using (var client = new HttpClient(httpClientHandler))
            {

                var str = string.Format(@"http://10.4.0.39:8080/ArcareAccount/Validate?account={0}&password={1}", account, password);
                HttpResponseMessage  res = client.GetAsync(str).Result;
                string result = JObject.Parse(await res.Content.ReadAsStringAsync())["result"].ToString();
                if(result == "True"){

                    var userRoles = await _dksDAO.GetRolesByAccount(account);

                    IEnumerable<string> onlyGroupNos = from u in userRoles 
                                        select u.GROUPNO ;
                    string roleArray = string.Join(".",onlyGroupNos);
                    var dict = new Dictionary<string, string>();
                    dict.Add("userId", userRoles[0].USERID.ToString());
                    dict.Add("user", userRoles[0].LOGIN);
                    dict.Add("role", roleArray);
                    dict.Add("factoryId", userRoles[0].FACTORYID);


                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(dict));
                }else{
                    return Ok(false);
                }


            }
           
        }        

    }
}
