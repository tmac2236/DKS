using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace DKS_API.Services.Interface
{
    public interface IFileService
    {
        Task<Boolean> SaveFiletoServer(IFormFile file, string settingNam, List<string> fileNames);
        FileInfo[] GetFileInfoByUrl(string folderPath);
        byte[] GetByteArrayByLocalUrlAddWaterMask(string folderPath, string stanLoveU);
        List<string> GetLocalPath(string settingNam, List<string> fileNames);
        byte[] AddWatermark(Byte[] stanIsBig,string stanLoveU);
        byte[] AddPdfWatermark(Byte[] stanIsBig,string stanLoveU);
        Task<string> UploadExcel(string dataUrl);
    }

}