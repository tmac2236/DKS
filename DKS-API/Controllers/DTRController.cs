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
        private readonly IModelDahDAO _modelDahDAO;
        private readonly IArticledDAO _articledDAO;
        private readonly IArticlePictureDAO _articlePictureDAO;
        private readonly IDevDtrFgtStatsDAO _devDtrFgtStatsDAO;
        private readonly IDevDtrVsFileDAO _devDtrVsFileDAO;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly ICommonService _commonService;
        private readonly ISendMailService _sendMailService;

        public DTRController(IMapper mapper, IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger,
         IDKSDAO dKSDAO, IDevDtrFgtResultDAO devDtrFgtResultDAO, IArticledDAO articledDAO, IDevDtrFgtStatsDAO devDtrFgtStatsDAO, IDevDtrVsFileDAO devDtrVsFileDAO,
         IArticlePictureDAO articlePictureDAO,IModelDahDAO modelDahDAO,
         IFileService fileService, IExcelService excelService,ICommonService commonService, ISendMailService sendMailService)
                : base(config, webHostEnvironment, logger)
        {
            _mapper = mapper;
            _dKSDAO = dKSDAO;
            _devDtrFgtResultDAO = devDtrFgtResultDAO;
            _articledDAO = articledDAO;
            _modelDahDAO = modelDahDAO;
            _devDtrFgtStatsDAO = devDtrFgtStatsDAO;
            _fileService = fileService;
            _excelService = excelService;
            _sendMailService = sendMailService;
            _commonService = commonService;            
            _devDtrVsFileDAO = devDtrVsFileDAO;
            _articlePictureDAO = articlePictureDAO;
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
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == article &&
                                 x.MODELNO.Trim() == modelNo &&
                                 x.MODELNAME.Trim() == modelName &&
                                 x.LABNO.Trim() == labNo &&
                                 x.STAGE.Trim() == stage);

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
                                 x.LABNO.Trim() == updateDevDtrFgtResultDto.LabNo);
            if (model != null)
            {
                model.RESULT = updateDevDtrFgtResultDto.Result;
                model.REMARK = updateDevDtrFgtResultDto.Remark;
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
                }
            }

            return Ok(model);
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
            Articled fromArt = _articledDAO.FindSingle(
                             x => x.PKARTBID.Trim() == transitArticleDto.PkArticle.Trim());

            // Step1: save fromArt to db (status =0 )
            fromArt.MODELNO = transitArticleDto.ModelNo.Trim();
            fromArt.FACTORYID = transitArticleDto.FactoryId;
            fromArt.STATUS = status;
            fromArt.MDUSERID =  transitArticleDto.UpdateUser.ToDecimal();
            fromArt.CHANGDATE = DateTime.Now;

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
            fromArtPic.FKARTICID = newPkArticle; 
            _articlePictureDAO.Add(fromArtPic);                           
            await _articlePictureDAO.SaveAll();  
            _logger.LogInformation(String.Format(@"****** Save ARTICLEPICTURE Success!! FKARTICID: {0} ******",newPkArticle)); 
            // Step3: if the new article don't have model in db copy one.
            ModelDah toModel = _modelDahDAO.FindSingle(
                             x => x.MODELNO.Trim() == transitArticleDto.ModelNo.Trim() &&
                                  x.FACTORYID.Trim() == transitArticleDto.FactoryId.Trim() );            
            if(toModel == null){
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
            }
            //step4: stend email
            /*
            var dksSignature = _config.GetSection("DksSignatureLine").Value;
            var content = string.Format(@"The Article : {0} was Transit from factory: {1}, please check it in F205 Article of the below website.{2}", transitArticleDto.Article, transitArticleDto.FactoryIdFrom , dksSignature);

            var toMails = new List<string>();
            
            toMails.Add(transitArticleDto.Email);
            await _sendMailService.SendListMailAsync(toMails, null, "A Article was Transit Please check it in F205 !", content, null);
             _logger.LogInformation(String.Format(@"******Sent Mail F205 Article Transit to : {0} ******", transitArticleDto.Email));
            */
            return  Ok(errMsg);
        }        
    }
}