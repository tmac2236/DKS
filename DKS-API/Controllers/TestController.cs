using Aspose.Cells;
using DFPS.API.Data.Repository;
using DKS.API.Models.DKS;
using DKS_API.Controllers;
using DKS_API.Data;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFPS.API.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        private readonly IArticledLdtmDAO _dao;
        private readonly IArticledDAO _articledDAO;
        private readonly IDKSDAO _dksDao;
        public TestController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<TestController> logger,
                IArticledLdtmDAO dao, IArticledDAO articledDAO, IDKSDAO dksDao)
        : base(config, webHostEnvironment, logger)
        {
            _dao = dao;
            _articledDAO = articledDAO;
            _dksDao = dksDao;
        }

        [AllowAnonymous]
        [HttpGet("testGetFile")]
        public IActionResult testGetFile()
        {
            _logger.LogInformation(String.Format(@"****** TestController testGetFile fired!! ******"));

            ArticledLdtm model = _dao.FindSingle(x => x.PKARTBID == "CD0000020155");
            byte[] result = model.ATTACHED_DATA;

            return File(result, "", model.ATTACHED_DATA_NAME);
        }
        [AllowAnonymous]
        [HttpGet("getSeasonNum")]
        public async Task<IActionResult> GetSeasonNum()
        {
            _logger.LogInformation(String.Format(@"****** TestController GetSeasonNum fired!! ******"));

            var data = await _articledDAO.GetSeasonNumDto();
            return Ok(data);
        }
        [AllowAnonymous]
        [HttpGet("getTransferToDTR")]
        public IActionResult GetTransferToDTR()
        {
            _dksDao.GetTransferToDTR("C", "U", "ART005");
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("crmGenerator")]
        public IActionResult CRMGenerator(string user, string subject, string itPic, string priority)
        {
            string content = "";
            string nowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            switch (priority)
            {
                case "1":
                    content = string.Format("關於{0}的意見有關於:{1}，\r\n非常感謝{0}的本次來信並且和我們分享您的感受，\r\n系統使用者的建議都是我們持續前進的動力，\r\n若有其他系統中需協助的部分，還請再次來信說明，\r\n資訊中心會盡快協助您確認與處理，謝謝。 Best Regards IT {2} 敬上。 於 {3}", user, subject, itPic, nowStr);
                    break;
                case "2":
                    content = string.Format("關於{0}的意見有關於:{1}，\r\n此部分服務人員已協助紀錄，\r\n後續必定會提供給營運與開發團隊進行參考，\r\n非常感謝{0}的本次來信並且和我們分享您的感受，\r\n系統使用者的建議都是我們持續前進的動力，\r\n若有其他系統中需協助的部分，還請再次來信說明，\r\n資訊中心會盡快協助您確認與處理，謝謝。 Best Regards IT {2} 敬上。 於 {3}", user, subject, itPic, nowStr);
                    break;
                case "3":
                    content = string.Format("關於{0}的意見有關於:{1}，\r\n非常感謝{0}的本次來信並且提供的建議與指教，\r\n專員已將您的想法確實傳達給開發及營運團隊，\r\n並將會作為今後開發和規劃時的參考，\r\n若有其他系統中需協助的部分，還請再次來信說明，\r\n資訊中心會盡快協助您確認與處理，謝謝。 Best Regards IT {2} 敬上。 於 {3}", user, subject, itPic, nowStr);
                    break;
            }
            return Ok(content);
        }
        [AllowAnonymous]
        [HttpGet("detectPigLeg")]
        public IActionResult DetectPigLeg()
        {
            string cYear = DateTime.Now.ToString("yyyy");
            string cMonth = DateTime.Now.ToString("MM");
            string filePath = string.Format(@"\\10.4.0.8\Apply_Form\General Affair-new\菜譜與圖書\{0}.{1}月菜單.xls",cYear,cMonth);
            Workbook workbook = new Workbook(filePath);
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                // 在这里处理每个工作表，例如获取单元格值
                Cells cells = worksheet.Cells;
                for (int row = 0; row < cells.MaxDataRow + 1; row++)
                {
                    for (int col = 0; col < cells.MaxDataColumn + 1; col++)
                    {
                        Cell cell = cells[row, col];
                        string cellValue = cell.StringValue;
                        Console.WriteLine($"工作表：{worksheet.Name}，行：{row}，列：{col}，值：{cellValue}");
                    }
                }
            }
            workbook.Dispose();
            return Ok();
        }

    }
}