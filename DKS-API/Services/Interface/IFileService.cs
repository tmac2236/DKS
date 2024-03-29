using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Aspose.Pdf;
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
        byte[] GetByteArrayByLocalUrlAddWaterMaskPDF(string folderPath, string stanLoveU);
        List<string> GetLocalPath(string settingNam, List<string> fileNames);
        byte[] AddWatermark(Byte[] stanIsBig,string stanLoveU);
        byte[] AddWatermarkPdf(string stanIsBig,string stanLoveU);
        Task<string> UploadExcel(string dataUrl);
        byte[] ConvertPDFtoWord(byte[] pdfByte);
        byte[] GeneratePDFExample();
        byte[] GenerateWordByTemp(string tempPath,DataTable dt);
    }

}