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
                _logger.LogError("!!!!!!CommonExportReportTabs have a exception!!!!!!");
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
                    designer.SetDataSource(string.Format(@"result{0}", index), data);
                    index++;
                }
                designer.Process();
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!CommonExportReportTabs have a exception!!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return stream.ToArray(); ;
        }
        public byte[] CommonExportReportTabs4F340PPD(List<object> dataList, string templateName)
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
                    designer.SetDataSource(string.Format(@"result{0}", index), data);
                    index++;

                }
                designer.Process();
                index = 0;
                foreach (object data in dataList)
                {
                    Worksheet ws = designer.Workbook.Worksheets[index];
                    //ws.Cells["W1"].PutValue("hello world");
                    int maxRow = dataList.Count + 2; //title have 2 rows
                    for (int i = 3; i <= maxRow; i++)
                    {

                        string photo = (string)ws.Cells["U" + i].Value; //If Photo Column is not empty or null 
                        if (!String.IsNullOrEmpty(photo))
                        {
                            string hyperlink = (string)ws.Cells["U" + i].Value;
                            ws.Cells["U" + i].PutValue("Click Me");
                            //Adding a hyperlink to a URL at "U"? cell
                            ws.Hyperlinks.Add("U" + i, 1, 1, hyperlink);
                        }
                        string pdf = (string)ws.Cells["V" + i].Value; //If Pdf Column is not empty or null
                        if (!String.IsNullOrEmpty(pdf))
                        {
                            string hyperlink = (string)ws.Cells["V" + i].Value;
                            ws.Cells["V" + i].PutValue("Click Me");
                            //Adding a hyperlink to a URL at "V"? cell
                            ws.Hyperlinks.Add("V" + i, 1, 1, hyperlink);
                        }
                        string fgtFileName = (string)ws.Cells["X" + i].Value; //If FgtFileName Column is not empty or null
                        if (!String.IsNullOrEmpty(fgtFileName))
                        {
                            string hyperlink = (string)ws.Cells["X" + i].Value;
                            string fgtResult = (string)ws.Cells["AZ" + i].Value;  //get the result string then empty AZ column  
                            ws.Cells["AZ" + i].PutValue("");                      //empty AZ column
                            ws.Cells["X" + i].PutValue(fgtResult);              
                            //Adding a hyperlink to a URL at "X"? cell
                            ws.Hyperlinks.Add("X" + i, 1, 1, hyperlink);
                        }

                        string article = (string)ws.Cells["F" + i].Value; 
                        if (String.IsNullOrEmpty(article))  //In order to exit loop, don't delete
                        {
                            break;
                        }
                    }

                    index++;
                }
                designer.Process();
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!CommonExportReportTabs4F340PPD have a exception!!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return stream.ToArray(); ;
        }
        public byte[] CommonExportReportWithATitle(object data, string templateName, string aTitle)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                string rootStr = _webHostEnvironment.ContentRootPath;
                var path = Path.Combine(rootStr, "Resources\\Template\\" + templateName);
                WorkbookDesigner designer = new WorkbookDesigner();
                designer.Workbook = new Workbook(path);
                Worksheet ws = designer.Workbook.Worksheets[0];
                designer.SetDataSource("title", aTitle);
                designer.SetDataSource("result", data);
                designer.Process();

                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!CommonExportReportTabs have a exception!!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return stream.ToArray(); ;
        }
    }
}