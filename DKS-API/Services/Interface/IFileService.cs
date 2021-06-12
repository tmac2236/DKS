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
        byte[] GetByteArrayByLocalUrl(string folderPath, int stanSize, string stanLoveU);
        List<string> GetLocalPath(string settingNam, List<string> fileNames);
        byte[] AddWatermark(Byte[] stanIsBig,int stanSize,string stanLoveU);
    }

}