using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.Services.Interface;

namespace DKS_API.Services.Interface
{
    public interface ISendMailService
    {
         Task SendListMailAsync(List<string> toMail,List<string>? toCCMail, string subject, string content, string? filePath);
         Task SendListMailAsyncbyByte(List<string> toMail, List<string>? toCCMail, string subject, string content, byte[] file);
         Task<bool> SendRFIDAlert();
    }
}