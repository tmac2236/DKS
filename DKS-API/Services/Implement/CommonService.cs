using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using DKS_API.Helpers;
using DKS_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Services.Implement
{
    public class CommonService : ICommonService
    {
        private readonly IConfiguration _config;
        private readonly IArticledDAO _articledDAO;
        private ILogger<F340CheckService> _logger;
        public CommonService(IConfiguration config, ILogger<F340CheckService> logger
            , IArticledDAO articledDAO)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _articledDAO = articledDAO;
        }

        public string GetPKARTBID()
        {
            _logger.LogInformation(String.Format(@"****** CommonController GetPKARTBID fired!! ******"));
            // CD0000023699
            string last =  _articledDAO.FindAll()
                        .OrderByDescending(x => x.PKARTBID).Take(1).Select(x => x.PKARTBID).ToList().FirstOrDefault();
            var number = last.Replace("CD","").ToInt();
            number += 1;
            var result = String.Format("{0:CD0000000000}",number);
            return result;
        }
    }
}