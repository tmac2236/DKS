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
using Microsoft.Extensions.Configuration;

namespace DFPS.API.Data.Repository
{
    public class DKSDAO : IDKSDAO
    {
        private readonly DKSContext _context;
        private readonly IConfiguration _config;
        private readonly string spCode;
        public DKSDAO(DKSContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
            spCode = _config.GetSection("AppSettings:SpCode").Value;
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

            //Stored Procedure A、B、C共用
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Season",sF340Schedule.season.Trim().ToUpper()),
                new SqlParameter("@CwaDateS",sF340Schedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340Schedule.cwaDateE)
            };
            string SQL ="";
            if(sF340Schedule.dataType =="DHO"){
                SQL = string.Format("EXECUTE dbo.GetF340Process_BuyPlan_C{0} @Season,@CwaDateS,@CwaDateE",spCode);
            }else if(sF340Schedule.dataType =="FHO"){

                pc.Add(new SqlParameter("@FactoryId", sF340Schedule.factory != null ? sF340Schedule.factory.Trim().ToUpper() : (object)DBNull.Value));
                pc.Add(new SqlParameter("@BuyPlanVer", sF340Schedule.bpVer.Trim()));
                SQL = string.Format("EXECUTE dbo.GetF340Process_BuyPlan_B{0} @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE",spCode);
            }else if(sF340Schedule.dataType =="DEV"){

                SQL =string.Format("EXECUTE dbo.GetF340Process_BuyPlan_A{0} @Season,@CwaDateS,@CwaDateE",spCode);
            }

            var data = await _context.GetF340ProcessView
                   .FromSqlRaw(SQL, pc.ToArray())
                   .ToListAsync();
            return data;
        }
        public PagedList<F340_ProcessDto> GetF340ProcessView(SF340Schedule sF340Schedule)
        {
            //Stored Procedure A、B、C共用
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Season",sF340Schedule.season.Trim().ToUpper()),
                new SqlParameter("@CwaDateS",sF340Schedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340Schedule.cwaDateE)
            };
            string SQL ="";
            if(sF340Schedule.dataType =="DHO"){
                SQL =string.Format("EXECUTE dbo.GetF340Process_BuyPlan_C{0} @Season,@CwaDateS,@CwaDateE",spCode);
            }else if(sF340Schedule.dataType =="FHO"){

                pc.Add(new SqlParameter("@FactoryId", sF340Schedule.factory != null ? sF340Schedule.factory.Trim().ToUpper() : (object)DBNull.Value));
                pc.Add(new SqlParameter("@BuyPlanVer", sF340Schedule.bpVer.Trim()));
                SQL =string.Format("EXECUTE dbo.GetF340Process_BuyPlan_B{0} @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE",spCode);
            }else if(sF340Schedule.dataType =="DEV"){

                SQL =string.Format("EXECUTE dbo.GetF340Process_BuyPlan_A{0} @Season,@CwaDateS,@CwaDateE",spCode);
            }

            var data = _context.GetF340ProcessView
                   .FromSqlRaw(SQL, pc.ToArray())
                   .ToList();

            return PagedList<F340_ProcessDto>
           .Create(data, sF340Schedule.PageNumber, sF340Schedule.PageSize, sF340Schedule.IsPaging);
        }

        public PagedList<F340_PpdDto> GetF340PPDView(SF340PPDSchedule sF340PPDSchedule)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryId",sF340PPDSchedule.factory != null ?sF340PPDSchedule.factory.Trim().ToUpper() : (object)DBNull.Value),
                new SqlParameter("@Season",sF340PPDSchedule.season.Trim().ToUpper()),
                new SqlParameter("@BuyPlanVer",sF340PPDSchedule.bpVer != "All" ? sF340PPDSchedule.bpVer.Trim() : (object)DBNull.Value ),
                new SqlParameter("@CwaDateS",sF340PPDSchedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340PPDSchedule.cwaDateE)
            };

            var data = _context.GetF340PpdView
                   .FromSqlRaw("EXECUTE dbo.GetF340_BuyPlan_PPD @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE", pc.ToArray())
                   .ToList();

            return PagedList<F340_PpdDto>
           .Create(data, sF340PPDSchedule.PageNumber, sF340PPDSchedule.PageSize, sF340PPDSchedule.IsPaging);
        }

        public async Task<List<F340_PpdDto>> GetF340PPDView4Excel(SF340PPDSchedule sF340PPDSchedule)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryId",sF340PPDSchedule.factory != "" ?sF340PPDSchedule.factory.Trim().ToUpper() : (object)DBNull.Value),
                new SqlParameter("@Season",sF340PPDSchedule.season.Trim().ToUpper()),
                new SqlParameter("@BuyPlanVer",sF340PPDSchedule.bpVer != "All" ? sF340PPDSchedule.bpVer.Trim() : (object)DBNull.Value ),
                new SqlParameter("@CwaDateS",sF340PPDSchedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340PPDSchedule.cwaDateE)
            };

            var data = await _context.GetF340PpdView
                   .FromSqlRaw("EXECUTE dbo.GetF340_BuyPlan_PPD @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE", pc.ToArray())
                   .ToListAsync();
            return data;
        }
    }
}