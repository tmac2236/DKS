using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using DKS_API.Services.Interface;
using DKS_API.Helpers;
using DKS_API.DTOs;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DKS_API.Controllers
{
    public class DTRController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IDKSDAO _dKSDAO;
        private readonly IDevDtrFgtResultDAO _devDtrFgtResultDAO;
        private readonly IArticledDAO _articledDAO;
        private readonly IDevDtrFgtStatsDAO _devDtrFgtStatsDAO;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        public DTRController(IMapper mapper, IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger,
         IDKSDAO dKSDAO, IDevDtrFgtResultDAO devDtrFgtResultDAO, IArticledDAO articledDAO, IDevDtrFgtStatsDAO devDtrFgtStatsDAO,
          IFileService fileService, IExcelService excelService)
                : base(config, webHostEnvironment, logger)
        {
            _mapper = mapper;
            _dKSDAO = dKSDAO;
            _devDtrFgtResultDAO = devDtrFgtResultDAO;
            _articledDAO = articledDAO;
            _devDtrFgtStatsDAO = devDtrFgtStatsDAO;
            _fileService = fileService;
            _excelService = excelService;
        }

        [HttpGet("getArticle4Fgt")]
        public async Task<IActionResult> GetArticle4Fgt(string modelNo, string article, string modelName)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetArticle4Fgt fired!! ******"));
            var data = await _articledDAO.GetArticleModelNameDto(modelNo, article, modelName);
            return Ok(data);
        }
        [HttpGet("getDevDtrFgtResultByModelArticle")]
        public async Task<IActionResult> GetDevDtrFgtResultByModelArticle([FromQuery] SDevDtrFgtResult sDevDtrFgtResult)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrFgtResultByModelArticle fired!! ******"));


            var data = await _dKSDAO.GetDevDtrFgtResultDto(sDevDtrFgtResult.article, sDevDtrFgtResult.modelNo, sDevDtrFgtResult.modelName);
            PagedList<DevDtrFgtResultDto> result = PagedList<DevDtrFgtResultDto>.Create(data, sDevDtrFgtResult.PageNumber, sDevDtrFgtResult.PageSize, sDevDtrFgtResult.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpGet("getPartName4DtrFgt")]
        public async Task<IActionResult> GetPartName4DtrFgt(string article, string stage)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetPartName4DtrFgt fired!! ******"));
            var data = await _devDtrFgtResultDAO.GetPartName4DtrFgt(article, stage);

            return Ok(data);
        }

        [HttpPost("editPdfDevDtrFgtResult")]
        public async Task<IActionResult> EditPdfDevDtrFgtResult()
        {

            _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult fired!! ******"));

            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            var modelName = HttpContext.Request.Form["modelName"].ToString().Trim();
            var labNo = HttpContext.Request.Form["labNo"].ToString().Trim();
            var fileName = HttpContext.Request.Form["fileName"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == article &&
                                 x.MODELNO.Trim() == modelNo &&
                                 x.MODELNAME.Trim() == modelName &&
                                 x.LABNO.Trim() == labNo);

            if (model == null) return NoContent();

            bool isUpdatePdf = false;
            if (fileName == "") //add a new
            {
                //Article + Stage + Kind + Test Result + LAB No .pdf
                fileName = string.Format("{0}_{1}_{2}_{3}_{4}.pdf", model.ARTICLE, model.STAGE, model.KIND, model.RESULT, model.LABNO);
            }
            else
            { //update and overwrite PDF
                isUpdatePdf = true;
            }

            // save or delete file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("QCTestResult");
            nastFileName.Add(article);
            nastFileName.Add(fileName);

            if (HttpContext.Request.Form.Files.Count > 0)       //save to server
            {
                var file = HttpContext.Request.Form.Files[0];
                if (await _fileService.SaveFiletoServer(file, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Add a PDF: {0}!! ******", fileName));
                    model.FILENAME = fileName;
                    model.UPUSR = loginUser;
                    model.UPDAY = DateTime.Now;

                    //add DTR FGT STATUS start
                    //find the status by article stage kind
                    var dtrFgtStats = _devDtrFgtStatsDAO.FindSingle(
                                    x => x.ARTICLE.Trim() == model.ARTICLE &&
                                    x.STAGE.Trim() == model.STAGE &&
                                    x.KIND.Trim() == model.KIND);

                    if (dtrFgtStats == null)
                    { //add status
                        dtrFgtStats = new DevDtrFgtStats();
                        dtrFgtStats.ARTICLE = model.ARTICLE;
                        dtrFgtStats.STAGE = model.STAGE;
                        dtrFgtStats.KIND = model.KIND;
                        dtrFgtStats.LABNO = model.LABNO;
                        dtrFgtStats.FILENAME = "";
                        if (model.RESULT == "PASS") dtrFgtStats.PASS = 1;
                        if (model.RESULT == "FAIL") dtrFgtStats.FAIL = 1;
                        _devDtrFgtStatsDAO.Add(dtrFgtStats);
                        _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Add a NEW DevDtrFgtStats: {0}!! ******", dtrFgtStats));
                    }
                    else
                    {
                        if (isUpdatePdf)
                        {  //overwrite pdf 
                           //won't update status
                            _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Overwrite a PDF: {0}!! ******", dtrFgtStats));
                        }
                        else
                        {   //same status but NEW LabNo   => update status
                            if (model.RESULT == "PASS") dtrFgtStats.PASS += 1;
                            if (model.RESULT == "FAIL") dtrFgtStats.FAIL += 1;
                            dtrFgtStats.LABNO = model.LABNO;
                            _devDtrFgtStatsDAO.Update(dtrFgtStats);
                            _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult ReCount result and  update LabNo: {0}!! ******", dtrFgtStats));

                        }
                        _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Update a Pdf BUT won't update DevDtrFgtStats: {0}!! ******", dtrFgtStats));
                    }
                    await _devDtrFgtStatsDAO.SaveAll();
                    //add DTR FGT STATUS end
                }
            }
            else
            {   //do CRUD-D here. delete from server

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Delete a PDF: {0}!! ******", fileName));
                    model.FILENAME = "";
                }
            }

            _devDtrFgtResultDAO.Update(model);
            await _devDtrFgtResultDAO.SaveAll();

            return Ok(model);
        }
        [HttpPost("addDevDtrFgtResult")]
        public async Task<IActionResult> AddDevDtrFgtResult(AddDevDtrFgtResultDto addDevDtrFgtResultDto)
        {
            _logger.LogInformation(String.Format(@"****** DTRController AddDevDtrFgtResult fired!! ******"));
            var devDtrFgtResult = _mapper.Map<DevDtrFgtResult>(addDevDtrFgtResultDto);
            if (devDtrFgtResult != null)
            {
                devDtrFgtResult.FILENAME = "";
                if(String.IsNullOrEmpty(devDtrFgtResult.PARTNO))devDtrFgtResult.PARTNO = "";
                if(String.IsNullOrEmpty(devDtrFgtResult.PARTNAME))devDtrFgtResult.PARTNAME = "";
                devDtrFgtResult.UPUSR = devDtrFgtResult.UPUSR;
                devDtrFgtResult.UPDAY = DateTime.Now;
                _devDtrFgtResultDAO.Add(devDtrFgtResult);
                await _devDtrFgtResultDAO.SaveAll();

                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPost("deleteDevDtrFgtResult")]
        public async Task<IActionResult> DeleteDevDtrFgtResult(AddDevDtrFgtResultDto addDevDtrFgtResultDto)
        {
            _logger.LogInformation(String.Format(@"****** DTRController DeleteDevDtrFgtResult fired!! ******"));
            var devDtrFgtResult = _mapper.Map<DevDtrFgtResult>(addDevDtrFgtResultDto);
            if (devDtrFgtResult != null)
            {
                //Step1: kill fgt result
                _devDtrFgtResultDAO.Remove(devDtrFgtResult);
                await _devDtrFgtResultDAO.SaveAll();
                //Step2: kill the pdf
                // save or delete file to server
                List<string> nastFileName = new List<string>();
                nastFileName.Add("QCTestResult");
                nastFileName.Add(devDtrFgtResult.ARTICLE);
                nastFileName.Add(devDtrFgtResult.FILENAME);
                //Step3:( if it have pdf) recount the pass or fault and previous labNo
                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController DeleteDevDtrFgtResult Delete a PDF: {0}!! ******", devDtrFgtResult.FILENAME));


                    //3.1 :find the status by article stage kind
                    var dtrFgtStats = _devDtrFgtStatsDAO.FindSingle(
                                    x => x.ARTICLE.Trim() == devDtrFgtResult.ARTICLE &&
                                    x.STAGE.Trim() == devDtrFgtResult.STAGE &&
                                    x.KIND.Trim() == devDtrFgtResult.KIND);
                    if (devDtrFgtResult.RESULT == "PASS") dtrFgtStats.PASS -= 1;
                    if (devDtrFgtResult.RESULT == "FAIL") dtrFgtStats.FAIL -= 1;
                    //3.2 find previous (the)
                    var thePrevious = _devDtrFgtResultDAO
                        .FindAll(x => x.ARTICLE.Trim() == devDtrFgtResult.ARTICLE &&
                                   x.STAGE.Trim() == devDtrFgtResult.STAGE &&
                                   x.KIND.Trim() == devDtrFgtResult.KIND &&
                                   x.FILENAME.Trim() != "")
                        .OrderByDescending(x => x.LABNO).Take(1).ToList().FirstOrDefault();
                    _logger.LogInformation(String.Format(@"******DTRController DeleteDevDtrFgtResult the Previous Lab No is : {0}!! ******", devDtrFgtResult.FILENAME));
                    if (thePrevious != null)
                    {
                        dtrFgtStats.LABNO = thePrevious.LABNO;
                    }
                    else
                    {
                        dtrFgtStats.LABNO = "";
                    }
                    _devDtrFgtStatsDAO.Update(dtrFgtStats);
                    await _devDtrFgtStatsDAO.SaveAll();
                }

                return Ok(true);
            }
            return Ok(false);
        }
        [HttpGet("getDevDtrFgtResultReport")]
        public async Task<IActionResult> GetDevDtrFgtResultReport([FromQuery] SDevDtrFgtResultReport sDevDtrFgtResultReport)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrFgtResultReport fired!! ******"));

            if (String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateS)) sDevDtrFgtResultReport.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateE)) sDevDtrFgtResultReport.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sDevDtrFgtResultReport.cwaDateS = sDevDtrFgtResultReport.cwaDateS.Replace("-", "/");
            sDevDtrFgtResultReport.cwaDateE = sDevDtrFgtResultReport.cwaDateE.Replace("-", "/");
            var data = await _dKSDAO.GetDevDtrFgtResultReportDto(sDevDtrFgtResultReport);
            PagedList<DevDtrFgtResultDto> result = PagedList<DevDtrFgtResultDto>.Create(data, sDevDtrFgtResultReport.PageNumber, sDevDtrFgtResultReport.PageSize, sDevDtrFgtResultReport.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }

        [HttpPost("exportDevDtrFgtResultReport")]
        public async Task<IActionResult> ExportDevDtrFgtResultReport(SDevDtrFgtResultReport sDevDtrFgtResultReport)
        {
            _logger.LogInformation(String.Format(@"****** DTRController ExportDevDtrFgtResultReport fired!! ******"));
            if (String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateS)) sDevDtrFgtResultReport.cwaDateS = _config.GetSection("LogicSettings:MinDate").Value;
            if (String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateE)) sDevDtrFgtResultReport.cwaDateE = _config.GetSection("LogicSettings:MaxDate").Value;
            sDevDtrFgtResultReport.cwaDateS = sDevDtrFgtResultReport.cwaDateS.Replace("-", "/");
            sDevDtrFgtResultReport.cwaDateE = sDevDtrFgtResultReport.cwaDateE.Replace("-", "/");
            var data = await _dKSDAO.GetDevDtrFgtResultReportDto(sDevDtrFgtResultReport);

            byte[] result = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            if(sDevDtrFgtResultReport.reportType =="Dev")
            {              //DEV
                result = _excelService.CommonExportReport(data, "TempDevDtrFgtResultReport_Dev.xlsx");
            }
            else if(sDevDtrFgtResultReport.reportType =="Buy Plan")
            {
                result = _excelService.CommonExportReport(data, "TempDevDtrFgtResultReport_BuyPlan.xlsx");
            }

            return File(result, "application/xlsx");
        }
        [HttpPost("addVSfile")]
        public async Task<IActionResult> AddVSfile([FromForm] DevDtrVisStandard devDtrVisStandard)
        {

            _logger.LogInformation(String.Format(@"******DTRController AddVSfile fired!! ******"));


            //記得修改DAO 
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == devDtrVisStandard.Article);

            //if (model == null) return NoContent();

            //Article + Stage + Kind + Test Result + LAB No .pdf
            var fileName = string.Format("{0}_{1}_{2}.pdf", devDtrVisStandard.Season,devDtrVisStandard.Article,devDtrVisStandard.Id);


            // save or delete file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("DTRVS");
            nastFileName.Add(devDtrVisStandard.Season);
            nastFileName.Add(devDtrVisStandard.Article);
            nastFileName.Add(fileName);

            if (devDtrVisStandard.File.Length > 0)       //save to server
            {
                if (await _fileService.SaveFiletoServer(devDtrVisStandard.File, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController AddVSfile Add a PDF: {0}!! ******", fileName));
                    //save to DAO
                }
            }


            return Ok(model);
        }
    }
}