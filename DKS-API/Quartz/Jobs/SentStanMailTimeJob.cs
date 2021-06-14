using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DKS_API.Services.Implement;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DFPS_API.Quartz.Jobs
{
    public class SentStanMailTimeJob : IJob
    {
        private ILogger<SentStanMailTimeJob> _logger;
        // 注入DI provider
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public SentStanMailTimeJob(ILogger<SentStanMailTimeJob> logger, IServiceProvider provider, IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
            _config = config;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string rootdir = Directory.GetCurrentDirectory();
                var localStr = _config.GetSection("AppSettings:ArticleUrl").Value;

                var theHour = DateTime.Now.Hour.ToString().Replace("0", "");
                var pathToSave = rootdir + localStr + theHour + ".jpg";
                pathToSave = pathToSave.Replace("DKS-API", "DKS-SPA");

                // 建立一個新的作用域
                using (var scope = _provider.CreateScope())
                {
                    string[] canWord ={"您上班辛苦了，記得多喝水 加油唷!",
                                   "不管此刻的你心情是如何，還想持續給你加油、打氣，讓你有力量快快忘卻不愉快的記憶！",
                                   "微風徐徐…感染陽光般的好心情∼工作再忙，也要起身動一動，好為精神打滿氣！",
                                   "透過電波傳遞Stan的話語，投遞Stan的關心！帶著滿心的幸福和快樂飄到你眼前，希望從這一秒開始，時時都能接收到這暖暖的溫度喔∼",
                                   "送你一顆星，使你上班順心，二顆心讓你上班好順心，三顆心讓你加班開心。加油唷~甘巴爹！",
                                   "也許，沒有最窩心的言語，但卻有顆最溫暖的心，在你感覺冷的時候，記得還有顆心讓你取暖，Stan在這給你加加油唷!",
                                   "一盤棋總有走得順利及不順利的時候，只要不灰心總是會有機會的，人生也是這樣，端看你有沒有心下完這盤棋！不要輕易放棄，加油！",
                                   "不論今天順利與否，希望你看到這則訊息時，心裡是暖暖的，嘴角是上揚的，盼能稍稍減緩你工作的疲累，忘卻煩惱天天都有好心情！",
                                   "讓Stan成為你打氣滴守護神，像是在旁滴為你加油，希望你在工作上一切順心如意。",
                                   "在你辛苦工作之餘，撥點時間看簡訊，你會收到Stan最真摯的祝福，累了、就休息，渴了、就喝杯水，煩了、就靜下來，想想Stan在為你加油！",
                                   "也許，沒有最窩心的言語，但~卻有顆最溫暖的心，在你忙了一整天之後，希望能帶給你最溫馨的祝福，早點休息！晚安唷！",
                                   "勇氣不可失，信心不可無，這世上沒有“能不能”的事，只怕你“肯不肯”！我們一起共勉之！",
                                   "雖然最近的工作變多了,壓力也更重了，但是我們仍然要繼續加油努力，因為我們要完成我們的夢想，即使很辛苦，也不可以放棄，讓我們一起加油！",
                                   "無論你今天遇到任何事，終將會風平浪靜！所以要過好每一個今天。Stan一定給你最大的勇氣讓你去面對任何的挑戰。"
                                   };
                    // Create a Random object  
                    Random rand = new Random();
                    // Generate a random index less than the size of the array.  
                    int index = rand.Next(canWord.Length);
                    // 解析你的作用域服務
                    var sendMailService = scope.ServiceProvider.GetService<ISendMailService>();
                    var title = string.Format(@"來自Stan的叮嚀，還剩{0}小時就下班囉，加油!", theHour);

                    var toMails = new List<string>();
                    toMails.Add("stan.chen@ssbshoes.com");
                    toMails.Add("sunshine@shc.ssbshoes.com");
                    toMails.Add("yvonne.liu@shc.ssbshoes.com");
                    toMails.Add("eddy.lee@ssbshoes.com");
                    var sign = "\r\n\r\n\r\n陳尚賢Stan Chen\r\n--------------------------------------------------------------------------------------------------------------------\r\nInformation and Technology Center (資訊中心)-ERP\r\nSHYANG SHIN BAO industrial co., LTD (翔鑫堡工業股份有限公司)\r\nSHYANG HUNG CHENG CO.,LTD (翔鴻程責任有限公司)\r\nTel: +84 (0274)3745-001-025 #6696\r\nEmail : Stan.Chen@ssbshoes.com";
                    var content = string.Format(@"{0}{1}", canWord[index], sign);

                    await sendMailService.SendListMailAsync(toMails, null, title, content, pathToSave);
                    _logger.LogError(title);
                }

                _logger.LogError("SentStanMailTimeJob was fired!!!!!");

                return;
            }
            catch (Exception e)
            {
                _logger.LogError("SentStanMailTimeJob have a exception!!!!!");
                _logger.LogError(e.ToString());
            }
        }
    }
}