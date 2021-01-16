using System.Collections.Generic;
using System.Threading.Tasks;
using DKS_API.DTOs;

namespace DKS_API.Data.Interface
{
    public interface ISPFactoryDAO
    {
        Task<IEnumerable<SelectLean>> GetAllLeans();
        Task<IEnumerable<SelectModelByLean>> GetModelsByLean(string leanId);
    }
}