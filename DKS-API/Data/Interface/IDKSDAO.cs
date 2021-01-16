using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;

namespace DKS_API.Data.Interface
{
    public interface IDKSDAO
    {
        Task<List<Ordsumoh>> SearchConvergence(string season, string stage);
        Task<Staccrth> SearchStaffByWorkNo(string workno);
        Task AddUserLogAsync(UserLog user);
        Task<bool> SaveAll();
    }
}