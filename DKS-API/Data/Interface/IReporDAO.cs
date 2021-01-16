using System.Collections.Generic;
using System.Threading.Tasks;
using DKS_API.DTOs;
using DKS_API.Helpers;

namespace DKS_API.Data.Interface
{
    public interface IReporDAO
    {
        Task<IEnumerable<GetReportDataPassDto>> GetReportDataPass(SReportDataPassDto sReportDataPassDto);
        Task<IEnumerable<PDModelDto>> GetPDModels(SPDModelDto sPDModelDto);
        PagedList<AttendanceDto> GetAttendances(SAttendanceDto sAttendanceDto);
        Task<IEnumerable<AttendanceDto>> GetAttendances();
        Task<IEnumerable<ChangeWorkerDto>> GetChangeWorkers(SPDModelDto sPDModelDto);
        Task<List<NoOperationDto>> GetNoOperations(string thedate);
    }
}