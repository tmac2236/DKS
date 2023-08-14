using System;
using System.Threading.Tasks;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using DKS_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DKS.API.Models.DKS;
using Microsoft.AspNetCore.Hosting;
using DKS_API.Services.Implement;
using DKS_API.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

namespace DKS_API.Controllers
{
    public class BomController : ApiController
    {
        private readonly IExcelService _excelService;
        private readonly ISendMailService _sendMailService;
        private readonly IFileService _fileService;
        private readonly IDevBomFileDAO _devBomFileDAO;
        private readonly IDKSDAO _dksDAO;

        public BomController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<WareHouseController> logger,
             IDevBomFileDAO devBomFileDAO,IDKSDAO dksDAO,
             IExcelService excelService, ISendMailService sendMailService, IFileService fileService)
        : base(config, webHostEnvironment, logger)
        {
            _excelService = excelService;
            _sendMailService = sendMailService;
            _fileService = fileService;
            _devBomFileDAO = devBomFileDAO;
            _dksDAO = dksDAO;

        }
        [HttpGet("getDevBomFileDetailDto")]
        public async Task<IActionResult> GetDevBomFileDetailDto([FromQuery] SDevBomFile sDevBomFile)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevBomFile fired!! ******"));
            var data =  await _dksDAO.GetDevBomFileDto(sDevBomFile);

            PagedList<DevBomFileDetailDto> result = PagedList<DevBomFileDetailDto>.Create(data, sDevBomFile.PageNumber, sDevBomFile.PageSize, sDevBomFile.IsPaging);
            Response.AddPagination(result.CurrentPage, result.PageSize,
            result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("addBOMfile")]
        public async Task<IActionResult> AddBOMfile([FromForm] UploadDevBomFileDto uploadDevBomFileDto)
        {

            _logger.LogInformation(String.Format(@"******BomController AddBOMfile fired!! ******"));


            // FactoryId + Season + Article + Id .pdf
            var fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}.xlsx", uploadDevBomFileDto.Season,uploadDevBomFileDto.Stage, uploadDevBomFileDto.Ver, uploadDevBomFileDto.ModelName,uploadDevBomFileDto.ModelNo,uploadDevBomFileDto.Article);

            // save file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("ArticleBoms");
            nastFileName.Add(uploadDevBomFileDto.Season);
            nastFileName.Add(uploadDevBomFileDto.Article);
            nastFileName.Add(fileName);

            DevBomFile model = new DevBomFile();
            if (uploadDevBomFileDto.File.Length > 0)       //save to server
            {
                if (await _fileService.SaveFiletoServer(uploadDevBomFileDto.File, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController AddVSfile Add a PDF: {0}!! ******", fileName));
                    //save to DAO
                    model.FACTORY = uploadDevBomFileDto.FactoryId;
                    model.DEVTEAMID = uploadDevBomFileDto.Team;
                    model.SEASON = uploadDevBomFileDto.Season;                    
                    model.MODELNO = uploadDevBomFileDto.ModelNo;
                    model.MODELNAME = uploadDevBomFileDto.ModelName;
                    model.ARTICLE = uploadDevBomFileDto.Article;

                    model.STAGE = uploadDevBomFileDto.Stage;
                    model.VER = uploadDevBomFileDto.Ver.ToShort();
                    model.FILENAME = fileName;

                    if (String.IsNullOrEmpty(uploadDevBomFileDto.Remark))
                    {
                        model.REMARK = "";
                    }
                    else
                    {
                        model.REMARK = uploadDevBomFileDto.Remark.Trim();
                    }
                    model.APPLY = "N";
                    model.UPUSR = uploadDevBomFileDto.UpdateUser;
                    model.UPDAY = DateTime.Now;
                    _devBomFileDAO.Add(model);
                    await _devBomFileDAO.SaveAll();
                }
            }

            return Ok(model);
        }    
        [HttpPost("applyBOMfile")]
        public async Task<IActionResult> ApplyBOMfile()
        {

            _logger.LogInformation(String.Format(@"******DTRController ApplyBOMfile fired!! ******"));

            var factoryId = HttpContext.Request.Form["factoryId"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var ver = HttpContext.Request.Form["ver"].ToShort();
            var remark = HttpContext.Request.Form["remark"].ToString().Trim();
            var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();

            DevBomFile model = _devBomFileDAO.FindSingle(
                                 x => x.ARTICLE == article &&
                                 x.FACTORY == factoryId &&
                                 x.VER == ver );
            model.APPLY = "Y";
            model.REMARK = remark;
            model.UPDAY = DateTime.Now;
            model.UPUSR = loginUser;

            _devBomFileDAO.Update(model);
            await _devBomFileDAO.SaveAll();

            return Ok(model);
        }
        [HttpGet("getDevTeamByLoginDto")]
        public async Task<IActionResult> GetDevTeamByLoginDto(string login)
        {
            _logger.LogInformation(String.Format(@"****** BomController GetDevTeamByLoginDto fired!! ******"));
            var data =  await _dksDAO.GetDevTeamByLoginDto(login);
            return Ok(data);
        }

    }

}
