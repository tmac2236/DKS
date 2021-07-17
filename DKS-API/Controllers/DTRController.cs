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

namespace DKS_API.Controllers
{
    public class DTRController : ApiController
    {

        private readonly IDKSDAO _dKSDAO;
        private readonly IDevDtrFgtResultDAO _devDtrFgtResultDAO;
        private readonly IArticledDAO _articledDAO;
        private readonly IFileService _fileService;


        [HttpGet("getArticlebyModelNo")]
        public async Task<IActionResult> GetArticlebyModelNo(string modelNo)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetArticlebyModelNo fired!! ******"));


            var data = await _articledDAO.FindAll(x => x.MODELNO.StartsWith(modelNo)).ToListAsync();
            return Ok(data);
        }
        public DTRController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger,
         IDKSDAO dKSDAO, IDevDtrFgtResultDAO devDtrFgtResultDAO, IArticledDAO articledDAO,
          IFileService fileService)
                : base(config, webHostEnvironment, logger)
        {
            _dKSDAO = dKSDAO;
            _devDtrFgtResultDAO = devDtrFgtResultDAO;
            _articledDAO = articledDAO;
            _fileService = fileService;
        }

        [HttpGet("getDevDtrFgtResultByModelArticle")]
        public async Task<IActionResult> GetDevDtrFgtResultByModelArticle([FromQuery] SDevDtrFgtResult sDevDtrFgtResult)
        {
            _logger.LogInformation(String.Format(@"****** DTRController GetDevDtrFgtResultByModelArticle fired!! ******"));


            var data = await _dKSDAO.GetDevDtrFgtResultDto(sDevDtrFgtResult.article, sDevDtrFgtResult.modelNo);
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
            var fileName = HttpContext.Request.Form["fileName"].ToString().Trim();
            DevDtrFgtResult model = _devDtrFgtResultDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == article &&
                                 x.MODELNO.Trim() == modelNo &&
                                 x.MODELNAME.Trim() == modelName &&
                                 x.LABNO.Trim() == labNo);

            if (model == null) return NoContent();

            if (fileName == "")
            {
                //Article + Stage + Kind + Test Result + LAB No .pdf
                fileName = string.Format("{0}_{1}_{2}_{3}_{4}.pdf", model.ARTICLE, model.STAGE, model.KIND, model.RESULT, model.LABNO);
            }
            // save or delete file to server
            List<string> nastFileName = new List<string>();
            nastFileName.Add("QCTestResult");
            nastFileName.Add(article);
            nastFileName.Add(fileName);

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];
                if (await _fileService.SaveFiletoServer(file, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController EditPdfDevDtrFgtResult Add a PDF: {0}!! ******", fileName));
                    model.FILENAME = fileName;
                }
            }
            else
            {   //do CRUD-D here.

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
    }
}