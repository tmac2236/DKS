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
    public class SentRFIAlertJob : IJob
    {
        private ILogger<SentRFIAlertJob> _logger;
        // 注入DI provider
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public SentRFIAlertJob(ILogger<SentRFIAlertJob> logger, IServiceProvider provider, IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
            _config = config;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                
                _logger.LogInformation(String.Format(@"******   SentRFIAlertJob Execute()!!!! ******"));
                // 建立一個新的作用域
                using (var scope = _provider.CreateScope())
                {
                    // 解析你的作用域服務
                    var sendMailService = scope.ServiceProvider.GetService<ISendMailService>();
                    await sendMailService.SendRFIDAlert();
                }

                return;
            }
            catch (Exception e)
            {
                _logger.LogError("!!!!!!SentRFIAlertJob have a exception!!!!!!");
                _logger.LogError(e.ToString());
            }
        }
    }
}