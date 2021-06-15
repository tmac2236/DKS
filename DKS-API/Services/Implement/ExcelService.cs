using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Services.Implement
{
    public class ExcelService : IExcelService
    {

        private readonly IConfiguration _config;
        private ILogger<ExcelService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExcelService(IConfiguration config, ILogger<ExcelService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webHostEnvironment = webHostEnvironment;
        }
        public byte[] CommonExportReport(object data, string templateName)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                string rootStr = _webHostEnvironment.ContentRootPath;
                var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
                WorkbookDesigner designer = new WorkbookDesigner();
                designer.Workbook = new Workbook(path);
                Worksheet ws = designer.Workbook.Worksheets[0];
                designer.SetDataSource("result", data);
                designer.Process();

                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!CommonExportReportTabs have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return stream.ToArray(); ;
        }
        public byte[] CommonExportReportTabs(List<object> dataList, string templateName)
        {
            MemoryStream stream = new MemoryStream();
            try
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
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!CommonExportReportTabs have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return stream.ToArray(); ;
        }
    }
}