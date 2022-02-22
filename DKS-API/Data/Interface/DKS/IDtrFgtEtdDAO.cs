using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS.API.Models.DKSSys;

namespace DKS_API.Data.Interface
{
    public interface IDtrFgtEtdDAO : ICommonDAO<DtrFgtEtd>
    {
     Task<List< DtrFgtEtdDto>> GetDtrFgtEtdDto();
    }
}