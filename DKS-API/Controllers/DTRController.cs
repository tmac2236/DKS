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
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DKS_API.Controllers
{
    public class DTRController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IDKSDAO _dKSDAO;
        private readonly IDevDtrFgtResultDAO _devDtrFgtResultDAO;
        private readonly IModelDahDAO _modelDahDAO;
        private readonly IModelDabDAO _modelDabDAO;
        private readonly IArticledDAO _articledDAO;
        private readonly IArticlePictureDAO _articlePictureDAO;
        private readonly IDevDtrFgtStatsDAO _devDtrFgtStatsDAO;
        private readonly IDevDtrVsFileDAO _devDtrVsFileDAO;
        private readonly IDtrLoginHistoryDAO _dtrLoginHistoryDAO;

        private readonly IDevSendMailDAO _devSendMailDAO;

        private readonly IDtrFgtShoesDAO _dtrFgtShoesDAO;
        private readonly IDtrFgtEtdDAO _dtrFgtEtdDAO;

        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly ICommonService _commonService;
        private readonly ISendMailService _sendMailService;

        public DTRController(IMapper mapper, IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger,
         IDKSDAO dKSDAO, IDevDtrFgtResultDAO devDtrFgtResultDAO, IArticledDAO articledDAO, IDevDtrFgtStatsDAO devDtrFgtStatsDAO, IDevDtrVsFileDAO devDtrVsFileDAO,
         IArticlePictureDAO articlePictureDAO,IModelDahDAO modelDahDAO,IDtrLoginHistoryDAO dtrLoginHistoryDAO,IDevSendMailDAO devSendMailDAO,IDtrFgtShoesDAO dtrFgtShoesDAO,
         IDtrFgtEtdDAO dtrFgtEtdDAO,IModelDabDAO modelDabDAO,
         IFileService fileService, IExcelService excelService,ICommonService commonService, ISendMailService sendMailService)
                : base(config, webHostEnvironment, logger)
        {
            _mapper = mapper;
            _dKSDAO = dKSDAO;
            _devDtrFgtResultDAO = devDtrFgtResultDAO;
            _articledDAO = articledDAO;
            _modelDahDAO = modelDahDAO;
            _modelDabDAO = modelDabDAO;
            _devDtrFgtStatsDAO = devDtrFgtStatsDAO;
            _fileService = fileService;
            _excelService = excelService;
            _sendMailService = sendMailService;
            _commonService = commonService;            
            _devDtrVsFileDAO = devDtrVsFileDAO;
            _articlePictureDAO = articlePictureDAO;
            _dtrLoginHistoryDAO = dtrLoginHistoryDAO;
            _devSendMailDAO = devSendMailDAO;
            _dtrFgtShoesDAO = dtrFgtShoesDAO;
            _dtrFgtEtdDAO = dtrFgtEtdDAO;
        }

           
        [HttpGet("getDevDtrFgtResultByModelArticle")]
        public async Task<IActionResult> GetDevDtrFgtResultByModelArticle([FromQuery] SDevDtrFgtResult sDevDtrFgtResult)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrFgtResultByModelArticle fired!! ******"));


            var data = await _dKSDAO.GetDevDtrFgtResultDto(sDevDtrFgtResult.article, sDevDtrFgtResult.modelNo, sDevDtrFgtResult.modelName,sDevDtrFgtResult.factoryId);
            PagedList<DevDtrFgtResultDto> result = PagedList<DevDtrFgtResultDto>.Create(data, sDevDtrFgtResult.PageNumber, sDevDtrFgtResult.PageSize, sDevDtrFgtResult.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }


        [HttpPost("editPdfDevDtrFgtResult")]
        public async Task<IActionResult> EditPdfDevDtrFgtResult()
        {

            _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult fired!! ******"));

            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var modelNo = HttpContext.Request.Form["modelNo"].ToString().Trim();
            var modelName = HttpContext.Request.Form["modelName"].ToString().Trim();
            var labNo = HttpContext.Request.Form["labNo"].ToString().Trim();
            var stage = HttpContext.Request.Form["stage"].ToString().Trim();
            var kind = HttpContext.Request.Form["kind"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == article &&
                                 x.MODELNO.Trim() == modelNo &&
                                 x.MODELNAME.Trim() == modelName &&
                                 x.LABNO.Trim() == labNo &&
                                 x.STAGE.Trim() == stage &&
                                 x.KIND.Trim() == kind);

            if (model == null) return NoContent();
            if (HttpContext.Request.Form.Files.Count == 0) return NoContent();
            var file = HttpContext.Request.Form.Files[0];

            var fileType = Extensions.GenerateExtension(file.ContentType.ToString());
            var elderFileName = model.FILENAME;
            //Article + Stage + Kind + Test Result + LAB No .pdf
            var fileName = string.Format("{0}_{1}_{2}_{3}.{4}", model.ARTICLE, model.STAGE, model.KIND, model.LABNO, fileType);


            // save or delete file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("QCTestResult");
            nastFileName.Add(article);
            nastFileName.Add(fileName);
        
            //save to server
            if (await _fileService.SaveFiletoServer(file, "F340PpdPic", nastFileName))
            {
                _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Add a File: {0}!! ******", fileName));
                model.FILENAME = fileName;
                
                //Cancel FgtStats 2021/09/14 Aven
                /*
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
                    if (isUpdate)
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
                */
            }


            _devDtrFgtResultDAO.Update(model);
            await _devDtrFgtResultDAO.SaveAll();

            return Ok(model);
        }
        //add and upgrde version both use this method
        [HttpPost("addDevDtrFgtResult")]
        public async Task<IActionResult> AddDevDtrFgtResult(AddDevDtrFgtResultDto addDevDtrFgtResultDto)
        {
            _logger.LogInformation(String.Format(@"****** DTRController AddDevDtrFgtResult fired!! ******"));
            var devDtrFgtResult = _mapper.Map<DevDtrFgtResult>(addDevDtrFgtResultDto);
            if (devDtrFgtResult != null)
            {
                if (String.IsNullOrEmpty(devDtrFgtResult.FILENAME)) devDtrFgtResult.FILENAME = "";
                if (String.IsNullOrEmpty(devDtrFgtResult.PARTNO))   devDtrFgtResult.PARTNO = "";
                if (String.IsNullOrEmpty(devDtrFgtResult.PARTNAME)) devDtrFgtResult.PARTNAME = "";
                devDtrFgtResult.UPUSR = devDtrFgtResult.UPUSR;
                devDtrFgtResult.UPDAY = DateTime.Now;
                devDtrFgtResult.FIRSTUPDAY = DateTime.Now;  //
                devDtrFgtResult.FACTORYID = devDtrFgtResult.LABNO.Substring(0, 1);
                _devDtrFgtResultDAO.Add(devDtrFgtResult);
                await _devDtrFgtResultDAO.SaveAll();

                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPost("updateDevDtrFgtResult")]
        public async Task<IActionResult> UpdateDevDtrFgtResult(AddDevDtrFgtResultDto updateDevDtrFgtResultDto)
        {
            _logger.LogInformation(String.Format(@"****** DTRController UpdateDevDtrFgtResult fired!! ******"));
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == updateDevDtrFgtResultDto.Article &&
                                 x.MODELNO.Trim() == updateDevDtrFgtResultDto.ModelNo &&
                                 x.MODELNAME.Trim() == updateDevDtrFgtResultDto.ModelName &&
                                 x.LABNO.Trim() == updateDevDtrFgtResultDto.LabNo &&
                                 x.STAGE.Trim() == updateDevDtrFgtResultDto.Stage &&
                                 x.KIND.Trim() == updateDevDtrFgtResultDto.Kind);
            if (model != null)
            {
                model.RESULT = updateDevDtrFgtResultDto.Result;
                model.REMARK = updateDevDtrFgtResultDto.Remark;
                model.UPUSR = updateDevDtrFgtResultDto.Upusr;
                model.UPDAY = DateTime.Now;
                _devDtrFgtResultDAO.Update(model);
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

                    /*  Cancel FgtStats 2021/09/14 Aven
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
                    */
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
            if (sDevDtrFgtResultReport.reportType == "Dev")
            {              //DEV
                result = _excelService.CommonExportReport(data, "TempDevDtrFgtResultReport_Dev.xlsx");
            }
            else if (sDevDtrFgtResultReport.reportType == "Buy Plan")
            {
                result = _excelService.CommonExportReport(data, "TempDevDtrFgtResultReport_BuyPlan.xlsx");
            }

            return File(result, "application/xlsx");
        }
        [HttpGet("getAFgtByLabNo")]
        public IActionResult GetAFgtByLabNo(string labNo)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetAFgtByLabNo fired!! ******"));

            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.LABNO.Trim() == labNo );

            return Ok(model);
        }        
        [HttpGet("getDevDtrVsReport")]
        public async Task<IActionResult> GetDevDtrVsReport([FromQuery] SDevDtrVsReport sDevDtrVsReport)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrVsReport fired!! ******"));


            var data = await _devDtrVsFileDAO.FindAll(x => x.SEASON == sDevDtrVsReport.Season &&
                                                           x.ARTICLE == sDevDtrVsReport.Article &&
                                                           x.FACTORYID == sDevDtrVsReport.FactoryId).ToListAsync();
            PagedList<DevDtrVsFile> result = PagedList<DevDtrVsFile>.Create(data, sDevDtrVsReport.PageNumber, sDevDtrVsReport.PageSize, sDevDtrVsReport.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }
        [HttpGet("getDevDtrList")]
        public async Task<IActionResult> GetDevDtrList([FromQuery] SDevDtrVsList sDevDtrVsList)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrList fired!! ******"));


            var data = await _articledDAO.GetDevDtrVsListDto(sDevDtrVsList);
            PagedList<DevDtrVsListDto> result = PagedList<DevDtrVsListDto>.Create(data, sDevDtrVsList.PageNumber, sDevDtrVsList.PageSize, sDevDtrVsList.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }

        [HttpPost("addVSfile")]
        public async Task<IActionResult> AddVSfile([FromForm] DevDtrVisStandardDto devDtrVisStandardDto)
        {

            _logger.LogInformation(String.Format(@"******DTRController AddVSfile fired!! ******"));


            // FactoryId + Season + Article + Id .pdf
            var fileName = string.Format("{0}_{1}_{2}_{3}.pdf", devDtrVisStandardDto.FactoryId,devDtrVisStandardDto.Season, devDtrVisStandardDto.Article, devDtrVisStandardDto.Id);

            // save file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("DTRVS");
            nastFileName.Add(devDtrVisStandardDto.Season);
            nastFileName.Add(devDtrVisStandardDto.Article);
            nastFileName.Add(fileName);

            DevDtrVsFile model = new DevDtrVsFile();
            if (devDtrVisStandardDto.File.Length > 0)       //save to server
            {
                if (await _fileService.SaveFiletoServer(devDtrVisStandardDto.File, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController AddVSfile Add a PDF: {0}!! ******", fileName));
                    //save to DAO
                    model.FACTORYID = devDtrVisStandardDto.FactoryId;
                    model.SEASON = devDtrVisStandardDto.Season.Trim();
                    model.ARTICLE = devDtrVisStandardDto.Article.Trim();
                    model.ID = devDtrVisStandardDto.Id.Trim();
                    model.FILENAME = fileName;
                    if (String.IsNullOrEmpty(devDtrVisStandardDto.Remark))
                    {
                        model.REMARK = "";
                    }
                    else
                    {
                        model.REMARK = devDtrVisStandardDto.Remark.Trim();
                    }
                    model.UPUSR = devDtrVisStandardDto.LoginUser;
                    model.UPDAY = DateTime.Now;
                    _devDtrVsFileDAO.Add(model);
                    await _devDtrVsFileDAO.SaveAll();
                    await _dKSDAO.DoSsbDtrVsFileUpdate(devDtrVisStandardDto.FactoryId,devDtrVisStandardDto.Season.Trim(),devDtrVisStandardDto.Article.Trim());
                }
            }

            return Ok(model);
        }
        [HttpPost("copyeVSfile/{fileName}")]
        public async Task<IActionResult> CopyeVSfile(DevDtrVsFile dto, string fileName)
        {

            _logger.LogInformation(String.Format(@"******DKSController CopyeVSfile fired!! ******"));
            // generate local path
            List<string> nastFileName = new List<string>();
            nastFileName.Add("DTRVS");
            nastFileName.Add(dto.SEASON);
            nastFileName.Add(dto.ARTICLE);
            nastFileName.Add(dto.FILENAME);
            var pathList = _fileService.GetLocalPath("F340PpdPic", nastFileName);
            if (!Directory.Exists(pathList[0])) return Ok("The file is empty");

            List<string> l = fileName.Split(',').ToList();
            foreach(string i in l){
            //check the article last id
                int last =  _devDtrVsFileDAO.FindAll(x=>x.ARTICLE == i)
                            .OrderByDescending(x => x.ID).Select(x => x.ID).ToList().FirstOrDefault().ToInt();
                last += 1; 
            // ###### copy file start ###### 
            // generate local path
            List<string> newNastFileName = new List<string>();
            newNastFileName.Add("DTRVS");
            newNastFileName.Add(dto.SEASON);
            newNastFileName.Add(i);
            //C_SS22_GY0793_1.pdf
            var newFileName = String.Format("{0}_{1}_{2}_{3}.pdf",dto.FACTORYID,dto.SEASON,i,last.ToString() );
            newNastFileName.Add(newFileName);
            var newPathList = _fileService.GetLocalPath("F340PpdPic", newNastFileName);
            if (!Directory.Exists(newPathList[0]))
            {
                DirectoryInfo di = Directory.CreateDirectory(newPathList[0]);
            }
            System.IO.File.Copy(pathList[1], newPathList[1]); 
            // ###### copy file end ######
            _logger.LogInformation(String.Format(@"******DTRController AddVSfile Add a PDF: {0}!! ******", fileName));
            //save to DAO
            DevDtrVsFile model = new DevDtrVsFile();
            model.FACTORYID = dto.FACTORYID;
            model.SEASON = dto.SEASON;
            model.ARTICLE = i;
            model.ID = last.ToString();
            model.FILENAME = newFileName;
            model.REMARK = String.Format("{0}",dto.REMARK);

            model.UPUSR = dto.UPUSR;
            model.UPDAY = DateTime.Now;

            _devDtrVsFileDAO.Add(model);
            await _devDtrVsFileDAO.SaveAll();
            await _dKSDAO.DoSsbDtrVsFileUpdate(model.FACTORYID,model.SEASON,model.ARTICLE);            

            }

            return Ok();

        }        

        [HttpPost("deleteVSResult")]
        public async Task<IActionResult> DeleteVSResult(DevDtrVsFile devDtrVsFile)
        {
            _logger.LogInformation(String.Format(@"****** DTRController DeleteVSResult fired!! ******"));
            if (devDtrVsFile != null)
            {

                //Step1: kill the pdf
                List<string> nastFileName = new List<string>();
                nastFileName.Add("DTRVS");
                nastFileName.Add(devDtrVsFile.SEASON);
                nastFileName.Add(devDtrVsFile.ARTICLE);
                nastFileName.Add(devDtrVsFile.FILENAME);
                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController DeleteVSResult Delete a PDF: {0}!! ******", devDtrVsFile.FILENAME));
                    //Step2: kill fgt result
                    _devDtrVsFileDAO.Remove(devDtrVsFile);
                    await _devDtrVsFileDAO.SaveAll();
                    await _dKSDAO.DoSsbDtrVsFileUpdate(devDtrVsFile.FACTORYID,devDtrVsFile.SEASON,devDtrVsFile.ARTICLE);
                    return Ok(true);
                }
            }
            return Ok(false);
        }
        //轉單
        [HttpPost("transitArticle")]
        public async Task<IActionResult> TransitArticle(TransitArticleDto transitArticleDto)
        {
            string status = "0";
            string errMsg = "";
            _logger.LogInformation(String.Format(@"****** DKSController TransitArticle fired!! ******"));
            Articled checkArt = _articledDAO.FindSingle(
                             x => x.ARTICLE.Trim() == transitArticleDto.Article.Trim()&&
                                    x.STAGE.Trim() == transitArticleDto.Stage.Trim()&&
                                    x.FACTORYID.Trim() == transitArticleDto.FactoryId.Trim());            
            if (checkArt != null){
                errMsg ="The Article、Stage、Factory is exist. Please refresh page and try again.";
                return  Ok(errMsg);
            }
            
            Articled fromArt = _articledDAO.FindSingle(
                             x => x.PKARTBID.Trim() == transitArticleDto.PkArticle.Trim());

            // Step1: save fromArt to db (status =0 )
            fromArt.MODELNO = transitArticleDto.ModelNo.Trim();
            fromArt.STAGE = transitArticleDto.Stage.Trim();
            fromArt.FACTORYID = transitArticleDto.FactoryId;
            fromArt.STATUS = status;
            fromArt.MDUSERID =  transitArticleDto.UpdateUser.ToDecimal();
            fromArt.CHANGDATE = DateTime.Now;
            fromArt.INSERDATE = DateTime.Now;
            
                //fromArt.REMARK = string.Format("Transit From Factory: {0} ,Update User: {1}", transitArticleDto.FactoryIdFrom,transitArticleDto.UpdateUser);       
                //get new PKARTICLE
            var newPkArticle = _commonService.GetPKARTBID();
                    fromArt.PKARTBID = newPkArticle;
            _articledDAO.Add(fromArt);
            await _articledDAO.SaveAll();

            _logger.LogInformation(String.Format(@"****** Save ARTICLED Success!! PKARTICLE: {0} ******",newPkArticle));      
            // Step2: copy ARTICLE_PICTURE and save to DB
            ArticlePicture fromArtPic = _articlePictureDAO.FindSingle(
                             x => x.FKARTICID.Trim() == transitArticleDto.PkArticle.Trim());
                             
            if(fromArtPic != null){
                fromArtPic.FKARTICID = newPkArticle; 
                _articlePictureDAO.Add(fromArtPic);                           
                await _articlePictureDAO.SaveAll();  
            } 

            _logger.LogInformation(String.Format(@"****** Save ARTICLEPICTURE Success!! FKARTICID: {0} ******",newPkArticle)); 
            // Step3: if the new article don't have model in db copy one.
            ModelDah toModel = _modelDahDAO.FindSingle(
                             x => x.MODELNO.Trim() == transitArticleDto.ModelNo.Trim() &&
                                  x.FACTORYID.Trim() == transitArticleDto.FactoryId.Trim() );            
            if(toModel == null){
                //取得複製來源的Model
                ModelDah fromModel = _modelDahDAO.FindSingle(
                             x => x.MODELNO.Trim() == transitArticleDto.ModelNoFrom.Trim() &&
                                  x.FACTORYID.Trim() == transitArticleDto.FactoryIdFrom.Trim() );

                fromModel.MODELNO = transitArticleDto.ModelNo.Trim();
                fromModel.DEVTEAMID = transitArticleDto.DevTeamId;
                fromModel.DEVELOPERID = "";
                fromModel.FACTORYID =  transitArticleDto.FactoryId;
                fromModel.STATUS = status;
                fromModel.MDUSERID =  transitArticleDto.UpdateUser.ToDecimal();
                fromModel.CHANGDATE = DateTime.Now;                 
                _modelDahDAO.Add(fromModel);                           
                await _modelDahDAO.SaveAll(); 
                _logger.LogInformation(String.Format(@"****** Save ModelDah Success!! ModelNo: {0}, FactoryId: {1} ******", fromModel.MODELNO, fromModel.FACTORYID) ); 
                //  Step3-1: if save modelDah sucess then save modelDab.
                //取得複製來源的Model表身
                List<ModelDab> fromModelbList = _modelDabDAO.FindAll(
                             x => x.MODELNO.Trim() == transitArticleDto.ModelNoFrom.Trim()).ToList();
                //被複製者的Model表身                  
                List<ModelDab> toModelbList = _modelDabDAO.FindAll(
                             x => x.MODELNO.Trim() == transitArticleDto.ModelNo.Trim()).ToList();                 
                if(toModelbList.Count == 0 && fromModelbList.Count > 0 ){  //被複製者本身沒有 + 複製來源>0 才可以複製 

                    fromModelbList.ForEach(x =>{
                        ModelDab modelDab = new ModelDab();
                        modelDab.MODELNO = transitArticleDto.ModelNo.Trim();
                        modelDab.SHOESIZE = x.SHOESIZE;
                        _modelDabDAO.Add(modelDab);
                        _logger.LogInformation(String.Format(@"******Ready to Save ModelDab, ModelNo: {0}******", modelDab.MODELNO) ); 
                    });

                await _modelDabDAO.SaveAll(); 
                _logger.LogInformation(String.Format(@"****** Save ModelDab Success!! ModelNo: {0}, FactoryId: {1} ******", fromModel.MODELNO, fromModel.FACTORYID) ); 
                }
            }
            //step4: stend email
            /*
            //var dksSignature = _config.GetSection("DksSignatureLine").Value;
            var dksSignature = "";
            var content = string.Format(@"The Article : {0} was Transit from factory: {1}, please check it in F205 Article of the below website.{2}", transitArticleDto.Article, transitArticleDto.FactoryIdFrom , dksSignature);

            var toMails = new List<string>();
            
            toMails.Add(transitArticleDto.Email);
            await _sendMailService.SendListMailAsync(toMails, null, "A Article was Transit Please check it in F205 !", content, null);
             _logger.LogInformation(String.Format(@"******Sent Mail F205 Article Transit to : {0} ******", transitArticleDto.Email));
            */
            //step5 : insert to dtr_excel and dtr_cwa date
            _dKSDAO.GetTransferToDTR(transitArticleDto.FactoryIdFrom,transitArticleDto.FactoryId,transitArticleDto.Article.Trim());
            return  Ok(errMsg);
        }
        [HttpGet("getDtrLoginHistory")]
        public IActionResult GetDtrLoginHistory([FromQuery] SDtrLoginHistory sDtrLoginHistory)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDtrLoginHistory fired!! ******"));

            /*
            var data =  _dtrLoginHistoryDAO.FindAll(x => x.SystemName.Contains(sDtrLoginHistory.systemName));
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.account))){
                data = data.Where( x=>x.Account == sDtrLoginHistory.account);
            }            
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeS))){
                data = data.Where( x=>x.LoginTime >= sDtrLoginHistory.loginTimeS.ToDateTime());
            }
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeE))){
                data = data.Where( x=>x.LoginTime <= sDtrLoginHistory.loginTimeE.ToDateTime());
            } 
            */
            var data = _dtrLoginHistoryDAO.GetDtrLoginUserHistoryDto(sDtrLoginHistory);

            PagedList<DtrLoginUserHistoryDto> result = PagedList<DtrLoginUserHistoryDto>.Create(data, sDtrLoginHistory.PageNumber, sDtrLoginHistory.PageSize, sDtrLoginHistory.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }   
              
        [HttpPost("exportDtrLoginHistory")]
        public IActionResult ExportDtrLoginHistory(SDtrLoginHistory sDtrLoginHistory)
        {
            _logger.LogInformation(String.Format(@"****** DTRController ExportDtrLoginHistory fired!! ******"));

            // query data from database
            /*  
            var data =  _dtrLoginHistoryDAO.FindAll(x => x.SystemName.Contains(sDtrLoginHistory.systemName));
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.account))){
                data = data.Where( x=>x.Account == sDtrLoginHistory.account);
            }            
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeS))){
                data = data.Where( x=>x.LoginTime >= sDtrLoginHistory.loginTimeS.ToDateTime());
            }
            if (!(String.IsNullOrEmpty(sDtrLoginHistory.loginTimeE))){
                data = data.Where( x=>x.LoginTime <= sDtrLoginHistory.loginTimeE.ToDateTime());
            }
            */
            var data = _dtrLoginHistoryDAO.GetDtrLoginUserHistoryDto(sDtrLoginHistory);  

            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempDtrLoginHistory.xlsx");

            return File(result, "application/xlsx");
        } 
        //檢查是否為最後一個階段:  開發: CR2->SMS->CS1   量化: CS2->CS3
        [HttpGet("checkEditFgtIsValid")]
        public async Task<IActionResult> CheckEditFgtIsValid(string factoryId, string article,string stage, string kind)
        {
            _logger.LogInformation(String.Format(@"******DTRController CheckEditFgtIsValid fired!! ******"));
            var isValid = true;
            var result = await _devDtrFgtResultDAO.FindAll(x => x.ARTICLE == article
                                                    && x.KIND == kind
                                                    && x.LABNO.Substring(0,1) == factoryId)
                                    .ToListAsync();
            if(stage == "CR2" ){            //開發
                var a = result.FirstOrDefault( x=> x.STAGE == "SMS" || x .STAGE =="CS1");
                if(a != null) isValid = false;
            }else if (stage == "SMS"){      //開發
                var a = result.FirstOrDefault( x=> x .STAGE =="CS1");
                if(a != null) isValid = false;
            }else if (stage == "CS2"){      //量化
                var a = result.FirstOrDefault( x=> x .STAGE =="CS3");
                if(a != null) isValid = false;
            }
            return Ok(isValid);

        }
         //檢查是否Dtr是否有重複:check (type:article、modelNo、modelName)+ stage + kind + factoryId can not be duplicated
        [HttpGet("checkFgtIsValid")]
        public async Task<IActionResult> CheckFgtIsValid(string type, string typeVal, string stage,string kind, string factoryId)
        {
            _logger.LogInformation(String.Format(@"******DTRController CheckFgtIsValid fired!! ******"));
            var isValid = false;
            DevDtrFgtResult model = new DevDtrFgtResult();

            if(type == "Article" ){            
                model =  await _devDtrFgtResultDAO.FindAll(x => x.ARTICLE == typeVal
                                                    && x.STAGE == stage
                                                    && x.KIND == kind
                                                    && x.LABNO.Substring(0,1) == factoryId)
                                    .FirstOrDefaultAsync();
            }else if (type == "Model No"){      
                model =  await _devDtrFgtResultDAO.FindAll(x => x.MODELNO == typeVal
                                                    && x.STAGE == stage
                                                    && x.KIND == kind
                                                    && x.LABNO.Substring(0,1) == factoryId)
                                    .FirstOrDefaultAsync();
            }else if (type == "Model Name"){     
                model =  await _devDtrFgtResultDAO.FindAll(x => x.MODELNAME == typeVal
                                                    && x.STAGE == stage
                                                    && x.KIND == kind
                                                    && x.LABNO.Substring(0,1) == factoryId)
                                    .FirstOrDefaultAsync();
            }
     

            if(model == null ) isValid = true;
            return Ok(isValid);
        }    

        [HttpGet("qcSentMailDtrFgtResult")]
        public async Task<IActionResult> QcSentMailDtrFgtResult(string stage, string modelNo, string article, string labNo, string remark, string type, string reason)
        {

            _logger.LogInformation(String.Format(@"******DTRController SentMailF340PpdByArticle fired!! ******"));
            ModelDah modelDah = _modelDahDAO.FindSingle(
                    x => x.MODELNO.Trim() == modelNo.Trim() && x.FACTORYID.Trim() == labNo.Substring(0,1));
            var season = modelDah.SEASON;

            var toMails = new List<string>();
            List<BasicCodeDto> list017 = await _dKSDAO.GetBasicCodeDto("017");    //017 = 開發小組
            BasicCodeDto teamId = list017.FirstOrDefault( x=> x.Key == modelDah.DEVTEAMID.Trim() );
            if(stage == "CR1" ||stage == "CR2" || stage =="SMS" || stage =="CS1"){   //mail to DEV
                toMails = teamId.MemoZh3.Split(";").Where( x => x.Length > 5 ).ToList();
            }else if (stage == "CS2" || stage == "CS3"){    //mail to COMM
                toMails = teamId.MemoZh4.Split(";").Where( x => x.Length > 5 ).ToList();
            }

            var content = string.Format(@"Hi Team: 
{0} Test report has been evaluated from pass to fail or deleted, please check with QC team.
(Model name: {1}, Season: {2}, Model No: {3}, Article: {4})
Type: {5}。  Reason: {6}。 Remark: {7}
", stage, modelDah.MODELNAME, season, modelNo, article, type, reason, remark);

            await _sendMailService.SendListMailAsync(toMails, null, string.Format(@"Test report change result (Season: {0}, Stage: {1}, Model Name: {2}, Model No:{3}, Art:{4})", season, stage, modelDah.MODELNAME, modelNo, article), content, null);
            return Ok();

        }

        [HttpGet("getSampleTrackDto")]
        public async Task<IActionResult> GetSampleTrackDto([FromQuery] SSampleTrackReportDto sSampleTrackReportDto)
        {
            _logger.LogInformation(String.Format(@"****** DKSController GetSampleTrackDto fired!! ******"));

            var data = await _dKSDAO.GetSampleTrackDto();
            PagedList<SampleTrackReportDto> result = PagedList<SampleTrackReportDto>.Create(data, sSampleTrackReportDto.PageNumber, sSampleTrackReportDto.PageSize, sSampleTrackReportDto.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }
        
        [HttpPost("exportSampleTrackDto")]
        public async Task<IActionResult> ExportSampleTrackDto(SSampleTrackReportDto sSampleTrackReportDto)
        {
            _logger.LogInformation(String.Format(@"****** DTRController ExportSampleTrackDto fired!! ******"));

            var data = await _dKSDAO.GetSampleTrackDto();


            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempSampleTrack.xlsx");

            return File(result, "application/xlsx");

        }
        [HttpGet("sentMailSampleTrack")]
        public async Task<IActionResult> SentMailSampleTrack()
        {

            _logger.LogInformation(String.Format(@"******DTRController SentMailSampleTrack fired!! ******"));

            var content = string.Format(@"Hi Dev, 

Please urgently check if the test shoes in attached list are transferred to QC team or not, 
If no, please bring it to QC asap.
If yes, please check with QC.  

Thank you
");

            var toMails = new List<string>();
            
            var users = await _devSendMailDAO.FindAll(x => x.EMAIL_TYPE == "02" &&
                                                           x.STATUS == 1).ToListAsync();  
            users.ForEach(x =>
            {
                toMails.Add(x.EMAIL);
            });
            
            var data = await _dKSDAO.GetSampleTrackDto();
            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempSampleTrack.xlsx");
            //toMails.Add("stan.chen@ssbshoes.com");
            //toMails.Add("aven.yu@ssbshoes.com");
            await _sendMailService.SendListMailAsyncbyByte(toMails, null, "Test Shoe Has Not Completely Transferred Yet – from DEV to QC", content, result);
            return Ok(toMails);

        }            
        [HttpGet("getDtrFgtEtdDto")]
        public async Task<IActionResult> GetDtrFgtEtdDto([FromQuery] SDtrFgtShoes sDtrFgtShoes)
        {
            _logger.LogInformation(String.Format(@"****** DKSController GetDtrFgtEtdDto fired!! ******"));

            var data = await _dtrFgtEtdDAO.GetDtrFgtEtdDto();
            PagedList<DtrFgtEtdDto> result = PagedList<DtrFgtEtdDto>.Create(data, sDtrFgtShoes.PageNumber, sDtrFgtShoes.PageSize, sDtrFgtShoes.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }
        [HttpPost("exportDtrFgtEtdDto")]
        public async Task<IActionResult> ExportDtrFgtEtdDto(SDtrFgtShoes sDtrFgtShoes)
        {
            _logger.LogInformation(String.Format(@"****** DTRController ExportDtrFgtEtdDto fired!! ******"));

            var data = await _dtrFgtEtdDAO.GetDtrFgtEtdDto();


            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempDtrFgtEtd.xlsx");

            return File(result, "application/xlsx");

        }
        [HttpPost("editDtrFgtEtds/{userId}")]
        public async Task<IActionResult> EditDtrFgtEtds(List<DtrFgtEtdDto> dtos, string userId)
        {

            _logger.LogInformation(String.Format(@"******DTRController EditDtrFgtEtds fired!! ******"));

            var editCount = 0;
            foreach (DtrFgtEtdDto dto in dtos)
            {

                DtrFgtEtd model = _dtrFgtEtdDAO.FindSingle(
                                                 x => x.FACTORYID == dto.FactoryId &&
                                                 x.ARTICLE == dto.Article  &&
                                                 x.STAGE  == dto.Stage &&
                                                 x.TEST  == dto.Test &&
                                                 x.QC_RECEIVE  == dto.QcReceive );

                if (model != null)
                {
                    //假如 QC ETD 和 Remark和DB裡的一樣就不用修改
                    if(model.QC_ETD == dto.QcEtd && model.REMARK.Trim() == dto.Remark) continue;
                    _logger.LogInformation(String.Format(@"******DTRController EditDtrFgtEtds Update QC_ETD: {0}, REMARK: {1}  !! ******", dto.QcEtd, dto.Remark ) );
                    model.QC_ETD = dto.QcEtd;
                    model.REMARK = dto.Remark;
                    model.UPUSR = userId;
                    model.UPDAY = DateTime.Now;

                    _dtrFgtEtdDAO.Update(model);
                    await _dtrFgtEtdDAO.SaveAll();
                    //sync Dtr_Excel的欄位
                    await _dtrFgtEtdDAO.DoSsbDtrQcEtdUpdate(dto);
                }
            }


            return Ok(editCount);

        }
        [HttpGet("getDtrF206Bom")]
        public async Task<IActionResult> GetDtrF206Bom([FromQuery] SDtrF206Bom sDtrF206Bom)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDtrF206Bom fired!! ******"));

            var data = await _dKSDAO.GetDtrF206Bom(sDtrF206Bom.article);

            PagedList<DtrF206BomDto> result = PagedList<DtrF206BomDto>.Create(data, sDtrF206Bom.PageNumber, sDtrF206Bom.PageSize, sDtrF206Bom.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);

        }
        [HttpPost("exportDtrF206Bom")]
        public async Task<IActionResult> ExportDtrF206Bom(SDtrF206Bom sDtrF206Bom)
        {
            _logger.LogInformation(String.Format(@"****** DTRController ExportDtrF206Bom fired!! ******"));
            //10/27再改
            var data = await _dKSDAO.GetDtrF206Bom(sDtrF206Bom.article);
            byte[] result = _excelService.CommonExportReport(data.ToList(), "TempDtrF206Bom.xlsx");

            return File(result, "application/xlsx");

        }                 
        /*
        [HttpGet("syncPlmArticleModel")]
        public async Task<IActionResult> SyncPlmArticleModel()
        {

            _logger.LogInformation(String.Format(@"******DTRController SyncPlmArticleModel fired!! ******"));
            DateTime dt =  DateTime.Now.AddMinutes(-300);
            var modelList = await _modelDahDAO.FindAll(x =>x.CHANGDATE >= dt)
                                        .Select(x => new {x.MODELNO,x.FACTORYID}).ToListAsync();
            var articleList = await _articledDAO.FindAll(x =>x.CHANGDATE >= dt)
                                        .Select(x => new {x.ARTICLE,x.FACTORYID,x.STAGE}).ToListAsync();
            List<dynamic> list = new List<dynamic>(); 
                list.Add(modelList);
                list.Add(articleList);
            var result = JsonConvert.SerializeObject(list);
            HttpClient client = new HttpClient();
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(result, Encoding.UTF8, "application/json");
            // Do the actual request and await the response
            var httpResponse = await client.PostAsync("http://localhost:5000/api/dtr/testt", httpContent);

        
            return Ok(result);

        }
        [HttpPost("testt")]
        public IActionResult Testt(object obj)
        {

            _logger.LogInformation(String.Format(@"******DTRController Testt fired!! ******"));

            return Ok(obj);

        }
        */                           

    }
}