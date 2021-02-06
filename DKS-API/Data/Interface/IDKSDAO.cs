using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;

namespace DKS_API.Data.Interface
{
    public interface IDKSDAO
    {
        Task<List<Ordsumoh>> SearchConvergence(string season, string stage);
        Task<Staccrth> SearchStaffByLOGIN(string login);
        Task<Staccrth> SearchStaffByUserId(string userId);
        Task AddUserLogAsync(UserLog user);
        Task<bool> SaveAll();
        F418_F420Dto GetF420F418View(string f418No);
        Task<List<F340_ProcessDto>> GetF340ProcessView(string season, string bpVer);
    }
}