using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Drawing;
using DKS.API.Models.DKS;
using DKS_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using DKS_API.Services.Interface;
using DKS_API.Helpers;

namespace DKS_API.Controllers
{
    public class PictureController : ApiController
    {
        private readonly IDKSDAO _dksDao;
        private readonly IFileService _fileService;

        public PictureController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PictureController> logger, IDKSDAO dksDao, IFileService fileService)
                : base(config, webHostEnvironment, logger)
        {
            _dksDao = dksDao;
            _fileService = fileService;
        }

        [HttpPost("deletePicByArticle")]
        public IActionResult DeletePicByArticle([FromForm] ArticlePic source)
        {
            _logger.LogInformation(String.Format(@"****** PictureController DeletePicByArticle fired!! ******"));

            string rootdir = Directory.GetCurrentDirectory();
            var localStr = _config.GetSection("AppSettings:ArticleUrl").Value;
            var pathToSave = rootdir + localStr + source.Article;
            pathToSave = pathToSave.Replace("DKS-API", "DKS-SPA");

            var fileName = source.Article + "_" + source.No + ".jpg";
            //新增檔名的全路徑
            var fullPath = Path.Combine(pathToSave, fileName);
            bool isExist = System.IO.File.Exists(fullPath);
            if (isExist)
            {
                string birdUrl = rootdir + "\\Resources\\article_null.jpg"; //讀取API的那張鳥圖

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    var fileStream = System.IO.File.OpenRead(birdUrl);
                    fileStream.CopyTo(stream);
                    fileStream.Close();
                }
                var staff = _dksDao.SearchStaffByLOGIN(source.User);
                UserLog userlog = new UserLog();
                userlog.PROGNAME = "F205";
                userlog.LOGINNAME = staff.Result.LOGIN;
                userlog.HISTORY = "Delete Picture " + fileName;
                userlog.UPDATETIME = DateTime.Now;
                _dksDao.AddUserLogAsync(userlog);

            }
            return Ok();
        }
        [HttpPost("uploadPicByArticle")]
        public IActionResult UploadPicByArticle([FromForm] ArticlePic source)
        {

            _logger.LogInformation(String.Format(@"****** PictureController UploadPicByArticle fired!! ******"));

            string rootdir = Directory.GetCurrentDirectory();
            var localStr = _config.GetSection("AppSettings:ArticleUrl").Value;
            var pathToSave = rootdir + localStr + source.Article;
            pathToSave = pathToSave.Replace("DKS-API", "DKS-SPA");

            //檔名含副檔名
            //var fileName = ContentDispositionHeaderValue.Parse(source.File.ContentDisposition).FileName.Trim('"');
            var fileName = source.Article + "_" + source.No + ".jpg";
            //新增檔名的全路徑
            var fullPath = Path.Combine(pathToSave, fileName);
            if (!Directory.Exists(pathToSave))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathToSave);
            }
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                source.File.CopyTo(stream);

                var staff = _dksDao.SearchStaffByLOGIN(source.User);
                UserLog userlog = new UserLog();
                userlog.PROGNAME = "F205";
                userlog.LOGINNAME = staff.Result.LOGIN;
                userlog.HISTORY = "Add Picture " + fileName;
                userlog.UPDATETIME = DateTime.Now;
                _dksDao.AddUserLogAsync(userlog);
            }
            return Ok();

        }
        [HttpGet("getPicByArticle")]
        public async Task<IActionResult> GetPicByArticle(string article)
        {

            _logger.LogInformation(String.Format(@"****** PictureController GetPicByArticle fired!! ******"));

            var localStr = _config.GetSection("AppSettings:ArticleUrl").Value;
            string folderPath = localStr + article;
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.jpg"); //Getting Text files
            List<Image> imgList = new List<Image>();

            foreach (FileInfo file in files)
            {
                imgList.Add(Image.FromFile(file.FullName));
            }

            Image combined;
            combined = imgList[0];

            if (imgList.Count > 1)
            {  //如果只有兩張以上就需要合併
                foreach (Image img in imgList)
                {
                    if (img == imgList[0]) continue;
                    combined = HorizontalMergeImages(combined, img);
                }

            }
            byte[] result = ImageToByteArray(combined);
            return File(result, "image/jpeg");
        }
        [HttpPost("uploadDtrQcPic")]
        public  IActionResult UploadDtrQcPic([FromBody] byte[] sFileName)
        {
            _logger.LogInformation(String.Format(@"****** PictureController UploadDtrQcPic fired!! ******"));
            /*
            var devSeason = HttpContext.Request.Form["devSeason"].ToString().Trim();
            var article = HttpContext.Request.Form["article"].ToString().Trim();
            var fileName = HttpContext.Request.Form["pdf"].ToString().Trim();
            //var loginUser = HttpContext.Request.Form["loginUser"].ToString().Trim();

            DateTime nowtime = DateTime.Now;
            var updateTimeStr = nowtime.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime updateTime = updateTimeStr.ToDateTime();


            if (fileName == "") //using in adding
            {
                //fileName + yyyy_MM_dd_HH_mm_ss_
                var formateDate = nowtime.ToString("yyyyMMddHHmmss");
                fileName = string.Format("{0}_{1}.jpg", article, formateDate);
            }

            List<string> nastFileName = new List<string>();
            nastFileName.Add("QCTestResult");
            nastFileName.Add(devSeason);
            nastFileName.Add(article);
            nastFileName.Add(fileName);


            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];
                if (await _fileService.SaveFiletoServer(file, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DKSController EditPicF340Ppd Add a Picture: {0}!! ******", fileName));
                }
            }
            else
            {   //do CRUD-D here.

                if (await _fileService.SaveFiletoServer(null, "F340PpdPic", nastFileName))
                {
                    _logger.LogInformation(String.Format(@"******DKSController EditPicF340Ppd Delete a Picture: {0}!! ******", fileName));
                }
            }
            */
            return Ok();

        }
        private Image HorizontalMergeImages(Image img1, Image img2)
        {
            _logger.LogInformation(String.Format(@"****** PictureController HorizontalMergeImages fired!! ******"));

            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            Wide = img1.Width + img2.Width;//設定寬度           
            if (img1.Height >= img2.Height)
            {
                High = img1.Height;
            }
            else
            {
                High = img2.Height;
            }
            Bitmap mybmp = new Bitmap(Wide, High);
            Graphics gr = Graphics.FromImage(mybmp);
            //處理第一張圖片
            gr.DrawImage(img1, 0, 0);
            //處理第二張圖片
            gr.DrawImage(img2, img1.Width, 0);
            MergedImage = mybmp;
            gr.Dispose();
            return MergedImage;
        }

        public byte[] ImageToByteArray(Image img)
        {
            _logger.LogInformation(String.Format(@"****** PictureController ImageToByteArray fired!! ******"));

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}