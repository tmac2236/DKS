using System;
using System.IO;
using DKS_API;
using DKS_API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;


namespace DFPS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string rootdir = Directory.GetCurrentDirectory();
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            try
            {
                //Aspoe-Excel
                Aspose.Cells.License cellLicense = new Aspose.Cells.License();
                string filePath = rootdir + "\\Resources\\" + "Aspose.Total.lic";
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                cellLicense.SetLicense(fileStream);
                fileStream.Close();
                //Aspoe-Pdf
                Aspose.Pdf.License pdfLicense = new Aspose.Pdf.License();
                FileStream fileStreampdf = new FileStream(filePath, FileMode.Open);
                pdfLicense.SetLicense(fileStreampdf);
                fileStreampdf.Close();
                //Aspoe-Word
                
                Aspose.Words.License wordLicense = new Aspose.Words.License();
                FileStream fileStreamword = new FileStream(filePath, FileMode.Open);
                wordLicense.SetLicense(fileStreamword);
                fileStreamword.Close();
                             
                //Initialize Logger
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(config)
                    .CreateLogger();
                Log.Information("DFPS Application Starting.......................");

                //Test t = new Test();
                //CSharpLab.Test();
                CreateHostBuilder(args).Build().Run();


            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
