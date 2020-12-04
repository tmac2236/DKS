using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DFPS.API.Data.Interface;
using DFPS.API.DTOs;
using DFPS_API.DTOs;
using System;
using DFPS_API.Helpers;

namespace DFPS_API.Data.Repository
{
    public class ReporDAO : IReporDAO
    {
        private readonly DataContext _context;
        public ReporDAO(DataContext context)
        {
            _context = context;
        }

        public async Task<List<NoOperationDto>> GetNoOperations(string thedate)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@StartDate",thedate)
            };
            var data = await _context.GetNoOperationDto
            .FromSqlRaw("EXECUTE dbo.GetNoOperationList_APP @StartDate", pc.ToArray())
            .ToListAsync();
            data = data.OrderBy(x =>x.StartDate).ToList();
            return data;

        }
        public async Task<IEnumerable<ChangeWorkerDto>> GetChangeWorkers(SPDModelDto sPDModelDto)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@StartDate",sPDModelDto.StartDate == null ? (object)DBNull.Value : sPDModelDto.StartDate),
                new SqlParameter("@EndDate",sPDModelDto.EndDate == null ? (object)DBNull.Value : sPDModelDto.EndDate),
                new SqlParameter("@TeamID",sPDModelDto.TeamID == null ? (object)DBNull.Value : sPDModelDto.TeamID)
            };
            var data = await _context.GetChangeWorkerDto
              .FromSqlRaw("EXECUTE dbo.QueryChangeWorker_ @StartDate, @EndDate, @TeamID", pc.ToArray())
              .ToListAsync();
            data = data.OrderBy(x =>x.StartDate).ToList();
            return data;
        }
        public PagedList<AttendanceDto> GetAttendances(SAttendanceDto sAttendanceDto)
        {
            var data = _context.GetAttendanceDto
                   .FromSqlRaw("EXECUTE dbo.AttendanceList_").AsEnumerable().OrderBy( x=>x.WorkDate ).ToList();
            return PagedList<AttendanceDto>
                .Create(data, sAttendanceDto.PageNumber, sAttendanceDto.PageSize, sAttendanceDto.IsPaging);
        }
        public async Task<IEnumerable<AttendanceDto>> GetAttendances()
        {
            var data = await _context.GetAttendanceDto
                   .FromSqlRaw("EXECUTE dbo.AttendanceList_").ToListAsync();
            data = data.OrderBy(x =>x.WorkDate).ToList();
            
            return data;
        }

        public async Task<IEnumerable<PDModelDto>> GetPDModels(SPDModelDto sPDModelDto)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@StartDate",sPDModelDto.StartDate == null ? (object)DBNull.Value : sPDModelDto.StartDate),
                new SqlParameter("@EndDate",sPDModelDto.EndDate == null ? (object)DBNull.Value : sPDModelDto.EndDate),
                new SqlParameter("@TeamID",sPDModelDto.TeamID == null ? (object)DBNull.Value : sPDModelDto.TeamID)
            };
            var data = await _context.GetPDModelDto
                   .FromSqlRaw("EXECUTE dbo.QueryPDModel_ @StartDate, @EndDate, @TeamID", pc.ToArray())
                   .ToListAsync();
            data = data.OrderBy(x =>x.StartDate).ToList();

            return data;
        }

        public async Task<IEnumerable<GetReportDataPassDto>> GetReportDataPass(SReportDataPassDto spParams)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@cdate",spParams.CDate == null ? (object)DBNull.Value : spParams.CDate),
                new SqlParameter("@cdate_e",spParams.CDateE == null ? (object)DBNull.Value : spParams.CDateE),
                new SqlParameter("@Line_ID",spParams.LineID == null ? (object)DBNull.Value : spParams.LineID),
                new SqlParameter("@Model_Name",spParams.ModelName == null ? (object)DBNull.Value : spParams.ModelName),
                new SqlParameter("@Model",spParams.Model == null ? (object)DBNull.Value : spParams.Model),
                new SqlParameter("@ProdNum",spParams.ProdNum == null ? (object)DBNull.Value : spParams.ProdNum),
                new SqlParameter("@ProdName_vn",spParams.ProdNameVn == null ? (object)DBNull.Value : spParams.ProdNameVn),
                new SqlParameter("@ProdName_tw",spParams.ProdNameTw == null ? (object)DBNull.Value : spParams.ProdNameTw),
                new SqlParameter("@WorkerNum",spParams.WorkerNum == null ? (object)DBNull.Value : spParams.WorkerNum),
                new SqlParameter("@WorkerName",spParams.WorkerName == null ? (object)DBNull.Value : spParams.WorkerName),
                new SqlParameter("@WorkLevel",spParams.WorkLevel == null ? (object)DBNull.Value : spParams.WorkLevel),
                new SqlParameter("@WorkLevel2",spParams.WorkLevel2 == null ? (object)DBNull.Value : spParams.WorkLevel2),
                new SqlParameter("@CheckPass",spParams.CheckPass == null ? (object)DBNull.Value : spParams.CheckPass)
            };
            List<GetReportDataPassDto> data = new List<GetReportDataPassDto>();
            try
            {
                if (spParams.CheckPass == "1")//通過
                {
                    data = await _context.GetReportDataPassDto
                   .FromSqlRaw("EXECUTE dbo.GetReportData_Pass_ @cdate, @cdate_e, @Line_ID, @Model_Name, @Model, @ProdNum, @ProdName_vn, @ProdName_tw,@WorkerNum, @WorkerName, @WorkLevel, @WorkLevel2, @CheckPass", pc.ToArray())
                   .ToListAsync();
                }
                else//未通過
                {
                    data = await _context.GetReportDataPassDto
                   .FromSqlRaw("EXECUTE dbo.GetReportData_ @cdate, @cdate_e, @Line_ID, @Model_Name, @Model, @ProdNum, @ProdName_vn, @ProdName_tw,@WorkerNum, @WorkerName, @WorkLevel, @WorkLevel2, @CheckPass", pc.ToArray())
                   .ToListAsync();
                }
                data = data.OrderBy(x =>x.CDate).ThenBy(x=>x.Maker).ToList();
            }
            catch (Exception ex)
            {
                string aa = ex.ToString();
            }


            return data;
        }

    }
}