using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.DTOs;
using DFPS_API.DTOs;
using DFPS_API.Helpers;

namespace DFPS.API.Data.Interface
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