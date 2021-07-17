using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Helpers;

namespace DKS_API.Data.Interface
{
    public interface IDKSDAO
    {
        Task<Staccrth> SearchStaffByLOGIN(string login);
        Task<Staccrth> SearchStaffByUserId(string userId);
        Task<List<UserRoleDto>> GetRolesByUserId(string userId);
        Task<List<UserRoleDto>> GetUsersByRole(string groupNo);
        Task AddUserLogAsync(UserLog user);
        Task<bool> SaveAll();
        F418_F420Dto GetF420F418View(string f418No);
        Task<List<F340_ProcessDto>> GetF340ProcessView(SF340Schedule sF340Schedule);
        //Task<List<F340_ProcessDto>> GetF340ProcessView4Excel(SF340Schedule sF340Schedule);
        Task<List<F340_PpdDto>> GetF340PPDView(SF340PPDSchedule sF340PPDSchedule);
        Task<List<DevDtrFgtResultDto>> GetDevDtrFgtResultDto(string article,string modelNo);
    }
}