using Microsoft.Extensions.Configuration;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DKS_API.Controllers
{
    public class CommonController : ApiController
    {
        private readonly IDKSDAO _dKSDAO;
        private readonly IArticledDAO _articledDAO;
        private readonly IDevDtrFgtResultDAO _devDtrFgtResultDAO;
        private readonly IDevBuyPlanDAO _devBuyPlanDAO;

        public CommonController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger,
         IDKSDAO dKSDAO, IArticledDAO articledDAO, IDevDtrFgtResultDAO devDtrFgtResultDAO, IDevBuyPlanDAO devBuyPlanDAO)
                : base(config, webHostEnvironment, logger)
        {
            _dKSDAO = dKSDAO;
            _articledDAO = articledDAO;
            _devDtrFgtResultDAO = devDtrFgtResultDAO;
            _devBuyPlanDAO = devBuyPlanDAO;
        }

        [HttpGet("getBasicCodeDto")]
        public async Task<IActionResult> GetBasicCodeDto(string typeNo)
        {
            _logger.LogInformation(String.Format(@"****** CommonController GetBasicCodeDto fired!! ******"));
            var data = await _dKSDAO.GetBasicCodeDto(typeNo);
            return Ok(data);
        }

        [HttpGet("getArticle")]
        public async Task<IActionResult> GetArticle(string modelNo, string article, string modelName)
        {
            _logger.LogInformation(String.Format(@"****** CommonController GetArticle fired!! ******"));
            var data = await _articledDAO.GetArticleModelNameDto(modelNo, article, modelName);
            return Ok(data);
        }
        [HttpGet("getArticleSeason")]
        public async Task<IActionResult> GetArticleSeason(string season, string article)
        {
            _logger.LogInformation(String.Format(@"****** CommonController GetArticleSeason fired!! ******"));
            var data = await _articledDAO.GetArticleSeasonDto(season, article);
            return Ok(data);
        }
        [HttpGet("getPartName")]
        public async Task<IActionResult> GetPartName(string article, string stage)
        {
            _logger.LogInformation(String.Format(@"****** CommonController GetPartName fired!! ******"));
            var data = await _devDtrFgtResultDAO.GetPartName4DtrFgt(article, stage);

            return Ok(data);
        }
        [HttpGet("getBPVersionBySeason")]
        public async Task<IActionResult> GetBPVersionBySeason(string season, string factory)
        {
            _logger.LogInformation(String.Format(@"******CommonController GetBPVersionBySeason fired!! ******"));

            var result = await _devBuyPlanDAO.FindAll(x => x.SEASON.Trim() == season.ToUpper().Trim()
                                                    && x.MANUF.Trim() == factory.ToUpper().Trim())
                                    .Select(x => new
                                    {
                                        VERN = x.VERN
                                    }).Distinct()
                                    .ToListAsync();

            List<string> bpVern = result.Select(x => x.VERN.ToString()).ToList();

            return Ok(bpVern);

        }

    }
}