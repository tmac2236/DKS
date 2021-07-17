using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS.API.Models.DKSSys;
using DKS_API.DTOs;

namespace DKS_API.Data.Interface
{
    public interface IDevDtrFgtResultDAO : ICommonDAO<DevDtrFgtResult>
    {
        Task<List<DevDtrFgtResult>> GetDevDtrFgtResultByModelArticle(string modelNo, string article);
    }

}