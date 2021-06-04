using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Services.Implement
{
    public class ExcelService : IExcelService
    {

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExcelService(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }
        public byte[] CommonExportReport(object data, string templateName)
        {

            string rootStr = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            return stream.ToArray(); ;
        }
        public byte[] CommonExportReportTabs(List<object> dataList, string templateName)
        {

            string rootStr = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            int index = 0;
            foreach (object data in dataList)
            {
                Worksheet ws = designer.Workbook.Worksheets[index];
                designer.SetDataSource("result", data);
                index++;
            }
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            return stream.ToArray(); ;
        }
    }
}