using System;
using System.Threading.Tasks;
using DKS_API.Services.Implement;
using DKS_API.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DFPS_API.Quartz.Jobs
{
    public class ShowDataTimeJob : IJob
    {
        private ILogger<ShowDataTimeJob> _logger;
        // 注入DI provider
        private readonly IServiceProvider _provider;
        public ShowDataTimeJob(ILogger<ShowDataTimeJob> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
        }
        public Task Execute(IJobExecutionContext context)
        {

            // 建立一個新的作用域
            using (var scope = _provider.CreateScope())
            {
                // 解析你的作用域服務
                var sendMailService = scope.ServiceProvider.GetService<ISendMailService>();
            }

            _logger.LogError("ShowDataTimeJob was fired!!!!!");

            return Task.CompletedTask;
        }
    }
}