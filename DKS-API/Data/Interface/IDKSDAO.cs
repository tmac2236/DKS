using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.Models.DKSSys;

namespace DFPS.API.Data.Interface
{
    public interface IDKSDAO
    {
        Task<List<Ordsumoh>> SearchConvergence(string season, string stage);

    }
}