using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.Services.Interface;

namespace DKS_API.Services.Interface
{
    public interface ISendMailService
    {
         Task SendListMailAsync(List<string> toMail, string subject, string content, string? filePath);
    }
}