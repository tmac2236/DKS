using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;

namespace DKS_API.Services.Interface
{
    public interface IF340CheckService
    {
        Task<string> IE340CheckMain();
        Task<List<string>> CheckToDoF340List();
        Task<bool> CheckPDM();
        Task<string> CheckF340TT();


    }
}