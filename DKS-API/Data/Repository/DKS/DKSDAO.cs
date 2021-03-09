using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DKS.API.Models.DKS;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.Data.SqlClient;
using System;
using DKS_API.Helpers;

namespace DFPS.API.Data.Repository
{
    public class DKSDAO : IDKSDAO
    {
        private readonly DKSContext _context;
        public DKSDAO(DKSContext context)
        {
            _context = context;
        }
        public async Task<List<Ordsumoh>> SearchConvergence(string season, string stage)
        {
            var list = await _context.ORDSUMOH.Where(x => x.SEASON == season && x.STAGE == stage).OrderBy(x => x.PRSUMNO).ToListAsync();
            return list;
        }
        //依LOGIN查帳號
        public async Task<Staccrth> SearchStaffByLOGIN(string login)
        {
            var staff = await _context.STACCRTH.FirstOrDefaultAsync(x => x.LOGIN == login.Trim());
            return staff;
        }
        //依UserID查帳號
        public async Task<Staccrth> SearchStaffByUserId(string userId)
        {
            var staff = await _context.STACCRTH.FirstOrDefaultAsync(x => x.USERID == Decimal.Parse(userId.Trim()));
            return staff;
        }
        //新增一筆user log 資料
        public async Task AddUserLogAsync(UserLog user)
        {
            _context.Add(user);
            await SaveAll();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public F418_F420Dto GetF420F418View(string f418No)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Proordno",f418No.Trim())
            };
            var data = _context.GetF420F418View
            .FromSqlRaw("EXECUTE dbo.GetF420NeedQty @Proordno", pc.ToArray()).ToList();

            return data[0];

        }

        public async Task<List<F340_ProcessDto>> GetF340ProcessView4Excel(SF340Schedule sF340Schedule)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryId",sF340Schedule.factory != "" ?sF340Schedule.factory.Trim().ToUpper() : (object)DBNull.Value),
                new SqlParameter("@Season",sF340Schedule.season.Trim().ToUpper()),
                new SqlParameter("@BuyPlanVer",sF340Schedule.bpVer != "All" ? sF340Schedule.bpVer.Trim() : (object)DBNull.Value ),
                new SqlParameter("@CwaDateS",sF340Schedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340Schedule.cwaDateE)
            };

            var data = await _context.GetF340ProcessView
                   .FromSqlRaw("EXECUTE dbo.GetF340Process_BuyPlan1 @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE", pc.ToArray())
                   .ToListAsync();
            return data;
        }
        public PagedList<F340_ProcessDto> GetF340ProcessView(SF340Schedule sF340Schedule)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryId",sF340Schedule.factory != null ?sF340Schedule.factory.Trim().ToUpper() : (object)DBNull.Value),
                new SqlParameter("@Season",sF340Schedule.season.Trim().ToUpper()),
                new SqlParameter("@BuyPlanVer",sF340Schedule.bpVer != "All" ? sF340Schedule.bpVer.Trim() : (object)DBNull.Value ),
                new SqlParameter("@CwaDateS",sF340Schedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340Schedule.cwaDateE)
            };

            var data =  _context.GetF340ProcessView
                   .FromSqlRaw("EXECUTE dbo.GetF340Process_BuyPlan1 @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE", pc.ToArray())
                   .ToList();

                 return PagedList<F340_ProcessDto>
                .Create(data, sF340Schedule.PageNumber, sF340Schedule.PageSize, sF340Schedule.IsPaging);
        }
    }
}