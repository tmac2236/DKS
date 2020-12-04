using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS_API.Helpers;

namespace DFPS_API.Services.Interface
{
    public interface ICommonService<T> where T : class
    {
        Task<bool> Add(T model);

        Task<bool> Update(T model);

        Task<bool> Delete(object id);

        Task<List<T>> GetAllAsync();

        Task<PagedList<T>> GetWithPaginations(PaginationParams param);

        Task<PagedList<T>> Search(PaginationParams param, object text);
        T GetById(object id);
    }
}