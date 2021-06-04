using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
                string rootdir = Directory.GetCurrentDirectory();
                var localStr = _config.GetSection("AppSettings:" + settingNam).Value;
                var pjName = _config.GetSection("AppSettings:ProjectName").Value;

                string innerPath = "";
                string fileName = string.Join("\\",fileNames.GetRange( fileNames.Count-1 , 1 ));
                //folder path
                if( fileNames.Count > 1 ){
                    innerPath = string.Join("\\",fileNames.GetRange( 0, fileNames.Count-1 ));
                }

                var pathToSave = rootdir + localStr + innerPath;    //新增資料夾的全路徑
                pathToSave = pathToSave.Replace(pjName + "-API", pjName + "-SPA");
                var fullPath = pathToSave + "\\" + fileName;   //新增檔名的全路徑
                if (file != null)
                {

                    if (!Directory.Exists(pathToSave))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathToSave);
                    }
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    isSuccess = true;
                }
                else
                {   //upload null present Delete
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
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
    }
}