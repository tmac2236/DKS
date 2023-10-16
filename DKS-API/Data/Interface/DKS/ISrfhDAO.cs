using System.Collections.Generic;
using DKS.API.Models.DKS;

namespace DKS_API.Data.Interface
{
    public interface ISrfhDAO : ICommonDAO<Srfh>
    {
        public List<SrfArticleDto> GetSrfArticleDto(string srfId);
    }
}