using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS.API.Models.DKSSys;
using DKS_API.DTOs;

namespace DKS_API.Data.Interface
{
    public interface IArticledDAO : ICommonDAO<Articled>
    {
        Task<List<ArticleModelNameDto>> GetArticleModelNameDto(string modelNo, string article, string modelName);
        Task<List<ArticleSeasonDto>> GetArticleSeasonDto(string season, string article);
        Task<List<DevDtrVsListDto>> GetDevDtrVsListDto(SDevDtrVsList sDevDtrVsList);
    }
}