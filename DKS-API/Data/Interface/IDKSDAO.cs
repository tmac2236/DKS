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
        Task<List<UserRoleDto>> GetRolesByAccount(string account);
        Task<List<UserRoleDto>> GetUsersByRole(string groupNo);
        Task<List<UserRoleDto>> GetUsersByName(string userName);
        Task AddUserLogAsync(UserLog user);
        Task<bool> SaveAll();
        F418_F420Dto GetF420F418View(string f418No);
        Task<List<F340_ProcessDto>> GetF340ProcessView(SF340Schedule sF340Schedule);
        //Task<List<F340_ProcessDto>> GetF340ProcessView4Excel(SF340Schedule sF340Schedule);
        Task<List<F340_PpdDto>> GetF340PPDView(SF340PPDSchedule sF340PPDSchedule);
        Task<List<DevDtrFgtResultDto>> GetDevDtrFgtResultDto(string article,string modelNo,string modelName, string factoryId);
        Task<List<DevDtrFgtResultDto>> GetDevDtrFgtResultReportDto(SDevDtrFgtResultReport sDevDtrFgtResultReport);
        Task<List<BasicCodeDto>> GetBasicCodeDto(string typeNo);
        void GetTransferToDTR(string factoryIdFrom,string factoryIdTo,string article);
        Task<List<SampleTrackReportDto>> GetSampleTrackDto();
        Task<List<NoneDto>> DoSsbDtrVsFileUpdate(string factoryId, string season, string article );

        Task<List<KanbanDataByLineDto>> GetKanbanDataByLineDto(string lineId );
        Task<List<KanbanTQCDto>> GetKanbanTQCDto(string lineId);
        Task<List<DtrF206BomDto>> GetDtrF206Bom(string article );
        Task<List<SrfChangeDto>> GetSrfChange(string article );
        Task<List<SrfDifferenceDto>> GetSrfDifference(string srfId1,string srfId2);
        Task<List<PrdEntryAccessDto>> GetPrdEntryAccessDto(string area,string recordTime);
        Task<List<PrdRfidAlertDto>> GetPrdRfidAlertDto(string recordTimeS,string recordTimeE);
        Task<List<BarcodeByCodeDto>> GetBarcodeByCodeDto(string code,string SDate,string EDate);
        Task<List<DevBomFileDetailDto>> GetDevBomFileDto(SDevBomFile sDevBomFile);
        Task<List<DevBomFileDetailDto>> GetDevBomFileNormalDto(SDevBomFile sDevBomFile);
        Task<List<DevTeamByLoginDto>> GetDevTeamByLoginDto(string login);
        Task<List<SsbGetHpSd138Dto>> GetSsbGetHpSd138Dto(string ecrNo);
        Task<List<DevBomDetailMailDto>> GetDevBomDetailMailDto(string factory,string article,string stage,short ver);
        Task<List<SendDevBomDetailMailListDto>> GetSendDevBomDetailMailListDto(string stage,string factory,string devTeamId);
        
    }
}