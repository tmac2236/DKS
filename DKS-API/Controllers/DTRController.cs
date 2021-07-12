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

namespace DKS_API.Controllers
{
    public class DTRController : ApiController
    {
        private readonly IDevDtrFgtDAO _devDtrFgtDAO;
        private readonly IFileService _fileService;

        public DTRController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger, IDevDtrFgtDAO devDtrFgtDAO, IFileService fileService)
                : base(config, webHostEnvironment, logger)
        {
            _devDtrFgtDAO = devDtrFgtDAO;
            _fileService = fileService;
        }
        [HttpPost("editDtrQc")]
        public async Task<IActionResult> EditDtrQc()
        {

            _logger.LogInformation(String.Format(@"******DTRController EditDtrQc fired!! ******"));

            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var stage = HttpContext.Request.Form["stage"].ToString().Trim();
            var kind = HttpContext.Request.Form["kind"].ToString().Trim();
            var vern = HttpContext.Request.Form["vern"].ToInt();
            var fileName = HttpContext.Request.Form["fileName"].ToString().Trim();
            DevDtrFgt model = _devDtrFgtDAO.FindSingle(
                                 x => x.ARTICLE.Trim() == article &&
                                 x.STAGE.Trim() == stage &&
                                 x.KIND.Trim() == kind &&
                                 x.VERN == vern);

            if(model == null) return NoContent();

            if (fileName == "")
            {
                fileName = string.Format("{0}_{1}_{2}_{3}.pdf", article, stage, kind, vern);
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
                    _logger.LogInformation(String.Format(@"******DTRController EditDtrQc Add a Picture: {0}!! ******", fileName));
                     model.FILENAME = fileName;
                }
            }
            else
            {   //do CRUD-D here.

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DTRController EditDtrQc Delete a Picture: {0}!! ******", fileName));
                    model.FILENAME = "";
                }
            }

            _devDtrFgtDAO.Update(model);
            await _devDtrFgtDAO.SaveAll();

            return Ok(model);
        }
    }
}