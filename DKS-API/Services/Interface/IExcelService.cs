using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace DKS_API.Services.Implement
{
    public interface IExcelService
    {
        byte[] CommonExportReport(object data, string templateName);
        byte[] CommonExportReportTabs(List<object> dataList, string templateName);
    }

}