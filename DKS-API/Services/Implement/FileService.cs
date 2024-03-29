using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Text;
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
        public byte[] GetByteArrayByLocalUrlAddWaterMask(string folderPath, string stanName)
        {
            byte[] stanIsBig = File.ReadAllBytes(folderPath);
            byte[] addWaterMask = AddWatermark(stanIsBig, stanName);
            return addWaterMask;
        }
        public byte[] GetByteArrayByLocalUrlAddWaterMaskPDF(string folderPath, string stanLoveU)
        {
            byte[] stanIsBig = File.ReadAllBytes(folderPath);
            byte[] addWaterMask = AddWatermark(stanIsBig, stanLoveU);
            return addWaterMask;
        }
        public byte[] AddWatermark(Byte[] originalByte, string stanName)
        {
            byte[] convertedToBytes;
            try
            {
                using (MemoryStream originalImageMemoryStream = new MemoryStream(originalByte))
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(originalImageMemoryStream))
                    {
                        int stanSize = (image.Height * image.Width) / 37400;
                        System.Drawing.Font font = new System.Drawing.Font("Arial", stanSize, System.Drawing.FontStyle.Italic, GraphicsUnit.Pixel);
                        System.Drawing.Color color = System.Drawing.Color.DarkGray;
                        System.Drawing.Point point = new System.Drawing.Point(image.Width / 10 * 1, (image.Height / 10 * 3));
                        System.Drawing.Point point1 = new System.Drawing.Point(image.Width / 10 * 1, (image.Height / 10 * 6));
                        System.Drawing.Point point2 = new System.Drawing.Point(image.Width / 10 * 1, (image.Height / 10 * 9));
                        System.Drawing.Point point3 = new System.Drawing.Point(image.Width / 10 * 1, (image.Height / 10 * 12));
                        System.Drawing.Point point4 = new System.Drawing.Point(image.Width / 10 * 1, (image.Height / 10 * 15));
                        SolidBrush brush = new SolidBrush(color);
                        using (Graphics graphics = Graphics.FromImage(image))
                        {
                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            stringFormat.LineAlignment = StringAlignment.Center;
                            var blankStr = "                 ";
                            graphics.RotateTransform(-45);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName), font, brush, point, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr), font, brush, point1, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName), font, brush, point2, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr), font, brush, point3, stringFormat);
                            graphics.DrawString(String.Format("{0}{1}{2}{3}{4}{5}{6}", stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName), font, brush, point4, stringFormat);
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
        public byte[] AddWatermarkPdf(string filePath, string stanName)
        {
            byte[] convertedToBytes;
            // Open document
            Document pdfDocument = new Document(filePath);
            var blankStr = "          ";
            var textStr1 = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName);
            var textStr2 = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr, stanName, blankStr);

            // Add stamp to each page of PDF
            //foreach (Page page in pdfDocument.Pages)
            for (int i = 1; i <= pdfDocument.Pages.Count; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    // Create text stamp
                    TextStamp textStamp;
                    if (j % 2 == 0)
                    {
                        textStamp = new TextStamp(textStr2);
                    }
                    else
                    {
                        textStamp = new TextStamp(textStr1);
                    }
                    // Set origin
                    textStamp.XIndent = 10;
                    textStamp.YIndent = (j * 100) - 500;

                    // Set text properties
                    textStamp.TextState.Font = FontRepository.FindFont("Arial");
                    textStamp.TextState.FontSize = 30.0F;
                    textStamp.TextState.FontStyle = FontStyles.Italic;
                    textStamp.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Gray);
                    textStamp.Opacity = 50;
                    textStamp.RotateAngle = 45;
                    pdfDocument.Pages[i].AddStamp(textStamp);
                }

            }

            using (MemoryStream updatedImageMemorySteam = new MemoryStream())
            {
                pdfDocument.Save(updatedImageMemorySteam, Aspose.Pdf.SaveFormat.Pdf);
                convertedToBytes = updatedImageMemorySteam.ToArray();
            }
            return convertedToBytes;
        }
        public byte[] ReduceImageSize(byte[] stanIsLong)
        {
            var jpegQuality = 50; //0~100
            System.Drawing.Image image;
            Byte[] outputBytes;
            try
            {
                using (var inputStream = new MemoryStream(stanIsLong))
                {
                    image = System.Drawing.Image.FromStream(inputStream);
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
        public byte[] ConvertPDFtoWord(byte[] pdfByte)
        {
            Byte[] outputBytes;
            using (var inputStream = new MemoryStream(pdfByte))
            {
                // Open the source PDF document
                Document pdfDocument = new Document(inputStream);
                // Save the file into MS document format
                using (var outputStream = new MemoryStream())
                {
                    pdfDocument.Save(outputStream, Aspose.Pdf.SaveFormat.Doc);
                    outputBytes = outputStream.ToArray();
                }
            }
            return outputBytes;
        }

        public byte[] GeneratePDFExample()
        {
            // Initialize document object
            Document document = new Document();
            // Add page
            Page page = document.Pages.Add();

            // -------------------------------------------------------------
            // Add image
            string rootdir = Directory.GetCurrentDirectory();
            string imageFileName = rootdir + "\\Resources\\taiwan.png"; //讀取API的那張鳥圖
            page.AddImage(imageFileName, new Aspose.Pdf.Rectangle(20, 730, 120, 830));

            // -------------------------------------------------------------
            // Add Header
            var header = new TextFragment("New ferry routes in Fall 2020");
            header.TextState.Font = FontRepository.FindFont("Arial");
            header.TextState.FontSize = 24;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.Position = new Position(130, 720);
            page.Paragraphs.Add(header);

            // Add description
            var descriptionText = "Visitors must buy tickets online and tickets are limited to 5,000 per day. Ferry service is operating at half capacity and on a reduced schedule. Expect lineups.";
            var description = new TextFragment(descriptionText);
            description.TextState.Font = FontRepository.FindFont("Times New Roman");
            description.TextState.FontSize = 14;
            description.HorizontalAlignment = HorizontalAlignment.Left;
            page.Paragraphs.Add(description);


            // Add table
            var table = new Table
            {
                ColumnWidths = "200",
                Border = new BorderInfo(BorderSide.Box, 1f, Aspose.Pdf.Color.DarkSlateGray),
                DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Aspose.Pdf.Color.Black),
                DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                Margin =
                {
                    Bottom = 10
                },
                DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Helvetica")
                }
            };

            var headerRow = table.Rows.Add();
            headerRow.Cells.Add("Departs City");
            headerRow.Cells.Add("Departs Island");
            foreach (Aspose.Pdf.Cell headerRowCell in headerRow.Cells)
            {
                headerRowCell.BackgroundColor = Aspose.Pdf.Color.Gray;
                headerRowCell.DefaultCellTextState.ForegroundColor = Aspose.Pdf.Color.WhiteSmoke;
            }

            var time = new TimeSpan(6, 0, 0);
            var incTime = new TimeSpan(0, 30, 0);
            for (int i = 0; i < 10; i++)
            {
                var dataRow = table.Rows.Add();
                dataRow.Cells.Add(time.ToString(@"hh\:mm"));
                time = time.Add(incTime);
                dataRow.Cells.Add(time.ToString(@"hh\:mm"));
            }

            page.Paragraphs.Add(table);

            Byte[] outputBytes;
            using (var outputStream = new MemoryStream())
            {
                document.Save(outputStream, Aspose.Pdf.SaveFormat.Pdf);
                outputBytes = outputStream.ToArray();
            }

            return outputBytes;
        }

        public byte[] GenerateWordByTemp(string tempPath,DataTable dt)
        {
            
            // For complete examples and data files, please go to https://github.com/aspose-words/Aspose.Words-for-.NET
            // The path to the documents directory.

            Aspose.Words.Document doc = new Aspose.Words.Document(tempPath);

            doc.Range.Replace("$NAME", "Hello World");
            doc.MailMerge.ExecuteWithRegions(dt);
            Byte[] outputBytes;
            using (var outputStream = new MemoryStream())
            {
                doc.Save(outputStream, Aspose.Words.SaveFormat.WordML);
                outputBytes = outputStream.ToArray();
            }

            return outputBytes;
        }
    }
}