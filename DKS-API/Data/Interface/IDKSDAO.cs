using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Helpers;

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
        PagedList<F340_ProcessDto> GetF340ProcessView(SF340Schedule sF340Schedule);
        Task<List<F340_ProcessDto>> GetF340ProcessView4Excel(SF340Schedule sF340Schedule);
        PagedList<F340_PpdDto> GetF340PPDView(SF340PPDSchedule sF340PPDSchedule);
        Task<List<F340_PpdDto>> GetF340PPDView4Excel(SF340PPDSchedule sF340PPDSchedule);
    }
}