using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using DKS_API.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        //constructor

        public ApiController(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }
        protected byte[] CommonExportReport(object data, string templateName)
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
        protected byte[] CommonExportReportTabs(List<object> dataList, string templateName)
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
                index ++;
            }
            designer.Process();
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            return stream.ToArray(); ;
        }
    }
}