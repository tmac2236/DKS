using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using Aspose.Cells;
using System;
using System.Threading.Tasks;
using System.Drawing;

namespace DFPS.API.Controllers
{
    public class PictureController : ApiController
    {
        private readonly IConfiguration _config;
        public PictureController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet("getPicByArticle")]
        public async Task<IActionResult> GetPicByArticle(string article)
        {
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

        private Image HorizontalMergeImages(Image img1, Image img2)
        {
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
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}