using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DKS_API.Services.Implement
{
    public class FileService : IFileService
    {

        private readonly IConfiguration _config;
        public FileService(IConfiguration config)
        {
            _config = config;
        }
        //儲存檔案到Server,If file is null resent to do Delete
        //file:檔案 
        //settingNam: root資料夾名稱
        //fileNames: 檔案名稱(含分層路徑)
        public async Task<Boolean> SaveFiletoServer(IFormFile file, string settingNam, List<string> fileNames)
        {
            Boolean isSuccess = false;
            try
            {
                var pathList = GetLocalPath(settingNam, fileNames);
                if (file != null)
                {

                    if (!Directory.Exists(pathList[0]))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathList[0]);
                    }
                    using (var stream = new FileStream(pathList[1], FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    isSuccess = true;
                }
                else
                {   //upload null present Delete
                    if (System.IO.File.Exists(pathList[1]))
                    {
                        System.IO.File.Delete(pathList[1]);
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return isSuccess;
            }

            return isSuccess;
        }
        //
        //settingNam: 指定folder名稱
        //fileNames: 資料夾路徑(可多層)
        //return: 取出新增資料夾的全路徑和檔名的全路徑
        public List<string> GetLocalPath(string settingNam, List<string> fileNames)
        {
            string rootdir = Directory.GetCurrentDirectory();
            var localStr = _config.GetSection("AppSettings:" + settingNam).Value;
            var pjName = _config.GetSection("AppSettings:ProjectName").Value;

            string innerPath = "";
            string fileName = string.Join("\\", fileNames.GetRange(fileNames.Count - 1, 1));
            //folder path
            if (fileNames.Count > 1)
            {
                innerPath = string.Join("\\", fileNames.GetRange(0, fileNames.Count - 1));
            }

            var pathToSave = rootdir + localStr + innerPath;    //新增資料夾的全路徑
            pathToSave = pathToSave.Replace(pjName + "-API", pjName + "-SPA");
            var fullPath = pathToSave + "\\" + fileName;   //新增檔名的全路徑
            List<string> result = new List<string>();
            result.Add(pathToSave);
            result.Add(fullPath);
            return result;
        }
        public FileInfo[] GetFileInfoByUrl(string folderPath)
        {

            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.jpg"); //Getting Text files

            return files;
        }
        public byte[] GetByteArrayByLocalUrl(string folderPath)
        {
            byte[] stanIsBig = File.ReadAllBytes(folderPath);
            byte[] addWaterMask = AddWatermark(stanIsBig);
            return addWaterMask;
        }
        public byte[] AddWatermark(Byte[] stanIsBig)
        {
            byte[] convertedToBytes;
            var byteSize = 50 ;

            using (MemoryStream originalImageMemoryStream = new MemoryStream(stanIsBig))
            {
                using (Image image = Image.FromStream(originalImageMemoryStream))
                {
                    Font font = new Font("Arial", byteSize, FontStyle.Italic, GraphicsUnit.Pixel);
                    Color color = Color.DarkGray;
                    Point point = new Point(image.Width / 10 * 1, (image.Height / 10 * 3));
                    Point point1 = new Point(image.Width / 10 * 1, (image.Height / 10 * 5));
                    Point point2 = new Point(image.Width / 10 * 1, (image.Height / 10 * 9));
                    Point point3 = new Point(image.Width / 10 * 1, (image.Height / 10 * 13));
                    Point point4 = new Point(image.Width / 10 * 1, (image.Height / 10 * 15));
                    SolidBrush brush = new SolidBrush(color);
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        var ssbStr = "copyright © SHYANG SHIN BAO IND. All Rights Reserved";
                        var blankStr = "                                     ";
                        graphics.RotateTransform(-45);
                        graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", ssbStr, blankStr, ssbStr, blankStr, ssbStr, blankStr, ssbStr), font, brush, point, stringFormat);
                        graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, ssbStr, blankStr, ssbStr, blankStr, ssbStr, blankStr), font, brush, point1, stringFormat);
                        graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", ssbStr, blankStr, ssbStr, blankStr, ssbStr, blankStr, ssbStr), font, brush, point2, stringFormat);
                        graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, ssbStr, blankStr, ssbStr, blankStr, ssbStr, blankStr), font, brush, point3, stringFormat);
                        graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", ssbStr, blankStr, ssbStr, blankStr, ssbStr, blankStr, ssbStr), font, brush, point4, stringFormat);
                    }

                    using (MemoryStream updatedImageMemorySteam = new MemoryStream())
                    {
                        image.Save(updatedImageMemorySteam, System.Drawing.Imaging.ImageFormat.Jpeg);
                        convertedToBytes = updatedImageMemorySteam.ToArray();
                    }
                }
            }

            return convertedToBytes;
        }
        public byte[] ReduceImageSize(byte[] stanIsLong)
        {
            var jpegQuality = 50;
            Image image;
            Byte[] outputBytes;
            using (var inputStream = new MemoryStream(stanIsLong))
            {
                image = Image.FromStream(inputStream);
                var jpegEncoder = ImageCodecInfo.GetImageDecoders()
                  .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, jpegEncoder, encoderParameters);
                    outputBytes = outputStream.ToArray();
                }
            }
            return outputBytes;
        }
    }
}