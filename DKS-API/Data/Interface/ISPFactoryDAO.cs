using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.DTOs;
using DFPS_API.DTOs;
using DFPS_API.Helpers;

namespace DFPS.API.Data.Interface
{
    public interface ISPFactoryDAO
    {
        Task<IEnumerable<SelectLean>> GetAllLeans();
        Task<IEnumerable<SelectModelByLean>> GetModelsByLean(string leanId);
    }
}