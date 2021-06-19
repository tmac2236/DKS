using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using DKS_API.Data.Interface;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DKS_API.Services.Implement
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _config;
        private ILogger<FileService> _logger;
        public FileService(IConfiguration config, ILogger<FileService> logger)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                _logger.LogError("!!!!!!SaveFiletoServer have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }

            return isSuccess;
        }
        //
        //settingNam: 指定folder名稱
        //fileNames: 資料夾路徑(可多層)
        //return: 取出新增資料夾的全路徑和檔名的全路徑
        public List<string> GetLocalPath(string settingNam, List<string> fileNames)
        {
            List<string> result = new List<string>();
            try
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

                result.Add(pathToSave);
                result.Add(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!GetLocalPath have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return result;
        }
        public FileInfo[] GetFileInfoByUrl(string folderPath)
        {

            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.jpg"); //Getting Text files

            return files;
        }
        public byte[] GetByteArrayByLocalUrlAddWaterMask(string folderPath, int stanSize, string stanLoveU)
        {
            byte[] stanIsBig = File.ReadAllBytes(folderPath);
            byte[] addWaterMask = AddWatermark(stanIsBig, stanSize, stanLoveU);
            return addWaterMask;
        }
        public byte[] AddWatermark(Byte[] stanIsBig, int stanSize, string stanLoveU)
        {
            byte[] convertedToBytes;
            try
            {
                using (MemoryStream originalImageMemoryStream = new MemoryStream(stanIsBig))
                {
                    using (Image image = Image.FromStream(originalImageMemoryStream))
                    {
                        System.Drawing.Font font = new System.Drawing.Font("Arial", stanSize, FontStyle.Italic, GraphicsUnit.Pixel);
                        Color color = Color.DarkGray;
                        Point point = new Point(image.Width / 10 * 1, (image.Height / 10 * 3));
                        Point point1 = new Point(image.Width / 10 * 1, (image.Height / 10 * 6));
                        Point point2 = new Point(image.Width / 10 * 1, (image.Height / 10 * 9));
                        Point point3 = new Point(image.Width / 10 * 1, (image.Height / 10 * 12));
                        Point point4 = new Point(image.Width / 10 * 1, (image.Height / 10 * 15));
                        SolidBrush brush = new SolidBrush(color);
                        using (Graphics graphics = Graphics.FromImage(image))
                        {
                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            stringFormat.LineAlignment = StringAlignment.Center;
                            var blankStr = "                 ";
                            graphics.RotateTransform(-45);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU), font, brush, point, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU, blankStr), font, brush, point1, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU), font, brush, point2, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU, blankStr), font, brush, point3, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU, blankStr, stanLoveU), font, brush, point4, stringFormat);
                        }

                        using (MemoryStream updatedImageMemorySteam = new MemoryStream())
                        {
                            image.Save(updatedImageMemorySteam, System.Drawing.Imaging.ImageFormat.Jpeg);
                            convertedToBytes = updatedImageMemorySteam.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!AddWatermark have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }

            return convertedToBytes;
        }
        public byte[] ReduceImageSize(byte[] stanIsLong)
        {
            var jpegQuality = 50; //0~100
            Image image;
            Byte[] outputBytes;
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError("!!!!!!ReduceImageSize have a exception!!!!!");
                _logger.LogError(ex.ToString());
                throw ex;
            }
            return outputBytes;
        }
        public Task<string> UploadExcel(string filePath)
        {
            // Instantiating a Workbook object
            Workbook workbook = new Workbook(filePath);
            var num = workbook.Worksheets.Count();
            throw new NotImplementedException();
        }
    }
}