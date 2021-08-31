using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.DTOs;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace DKS_API.Services.Interface
{
    public interface IExcelService
    {
        byte[] CommonExportReport(object data, string templateName);
        byte[] CommonExportReportTabs(List<object> dataList, string templateName);
        //only for F340PPD
        byte[] CommonExportReportTabs4F340PPD(List<List<F340_PpdDto>> dataList, string templateName);
        //if the export excel have a title use this one
        byte[] CommonExportReportWithATitle(object data, string templateName,string aTitle);
    }

}