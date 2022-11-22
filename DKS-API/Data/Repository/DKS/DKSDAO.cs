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
            spCode = _config.GetSection("AppSettings:SpCode").Value; //目前用在F340進度表而已
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

        public async Task<List<F340_ProcessDto>> GetF340ProcessView(SF340Schedule sF340Schedule)
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

            //tranfer minDate to ''
            string minDate = _config.GetSection("LogicSettings:MinDate").Value;
            data.ForEach(m =>{
                if(m.ActivationDate == minDate) m.ActivationDate = "";
            });
            return data;
        }

        public async Task<List<F340_PpdDto>> GetF340PPDView(SF340PPDSchedule sF340PPDSchedule)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryId",sF340PPDSchedule.factory),
                new SqlParameter("@Season",sF340PPDSchedule.season.Trim().ToUpper()),
                new SqlParameter("@BuyPlanVer",sF340PPDSchedule.bpVer == null || sF340PPDSchedule.bpVer == "" ? (object)DBNull.Value : sF340PPDSchedule.bpVer.Trim() ),
                new SqlParameter("@CwaDateS",sF340PPDSchedule.cwaDateS),
                new SqlParameter("@CwaDateE",sF340PPDSchedule.cwaDateE),
                new SqlParameter("@ModelNo",(sF340PPDSchedule.modelNo == null || sF340PPDSchedule.modelNo == "" ) ? (object)DBNull.Value : sF340PPDSchedule.modelNo.Trim()),
                new SqlParameter("@ModelName",(sF340PPDSchedule.modelName == null || sF340PPDSchedule.modelName == "" ) ? (object)DBNull.Value : sF340PPDSchedule.modelName.Trim()),
                new SqlParameter("@Article",(sF340PPDSchedule.article == null || sF340PPDSchedule.article == "" ) ? (object)DBNull.Value : sF340PPDSchedule.article.Trim()),
            };
            var data = await _context.GetF340PpdView
                   .FromSqlRaw(string.Format("EXECUTE dbo.GetF340_BuyPlan_PPD_FGT @FactoryId,@Season,@BuyPlanVer,@CwaDateS,@CwaDateE,@ModelNo,@ModelName,@Article"), pc.ToArray())
                   .ToListAsync();
            //tranfer minDate to ''
            string minDate = _config.GetSection("LogicSettings:MinDate").Value;
            data.ForEach(m =>{
                if(m.CwaDeadline == minDate) m.CwaDeadline = "";
            });
            return data;
        }

        public async Task<List<UserRoleDto>> GetRolesByUserId(string userId)
        {
            string strWhere = "WHERE A.USERID=" + userId ;
            string strSQL = string.Format(@"
                                            SELECT  
                                                   A.WORKPNO
                                                  ,CONVERT(varchar,A.USERID) AS USERID
                                                  ,A.LOGIN
                                                  ,A.FACTORYID
                                            	  ,B.GROUPNO
                                                  ,C.EMAIL
                                              FROM STACCRTH AS A
                                              LEFT JOIN UTL_PJOBINER AS B ON A.USERID = B.USERID
                                              INNER JOIN SSBDEV3..UTL_USERINFO AS C ON A.USERID = C.USERID ");
            strSQL += strWhere;
            var data = await _context.UserRoleDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }

        public async Task<List<UserRoleDto>> GetUsersByRole(string groupNo)
        {
            string strWhere = "WHERE B.GROUPNO = '" + groupNo +"'" ;
            string strSQL = string.Format(@"
                                            SELECT  
                                                   A.WORKPNO
                                                  ,CONVERT(varchar,A.USERID) AS USERID
                                                  ,A.LOGIN
                                                  ,A.FACTORYID
                                            	  ,B.GROUPNO
                                                  ,C.EMAIL
                                              FROM STACCRTH AS A
                                              LEFT JOIN UTL_PJOBINER AS B ON A.USERID = B.USERID
                                              INNER JOIN SSBDEV3..UTL_USERINFO AS C ON A.USERID = C.USERID ");
            strSQL += strWhere;
            var data = await _context.UserRoleDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
        public async Task<List<DevDtrFgtResultDto>> GetDevDtrFgtResultDto(string article,string modelNo,string modelName, string factoryId)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Article",  String.IsNullOrEmpty(article) ? (object)DBNull.Value : article.Trim()),
                new SqlParameter("@ModelNo",  String.IsNullOrEmpty(modelNo) ? (object)DBNull.Value : modelNo.Trim()),
                new SqlParameter("@ModelName",  String.IsNullOrEmpty(modelName) ? (object)DBNull.Value : modelName.Trim()),
                new SqlParameter("@Factory",  String.IsNullOrEmpty(factoryId) ? (object)DBNull.Value : factoryId.Trim()),
            };
            var data = await _context.GetDevDtrFgtResultDto
                   .FromSqlRaw("EXECUTE dbo.GetDTR_FGT_Result @Article,@ModelNo,@ModelName,@Factory", pc.ToArray())
                   .ToListAsync();

            return data;

        }
        public async Task<List<DevDtrFgtResultDto>> GetDevDtrFgtResultReportDto(SDevDtrFgtResultReport sDevDtrFgtResultReport)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Article",  String.IsNullOrEmpty(sDevDtrFgtResultReport.article) ? (object)DBNull.Value : sDevDtrFgtResultReport.article.Trim().ToUpper()),
                new SqlParameter("@ModelNo",  String.IsNullOrEmpty(sDevDtrFgtResultReport.modelNo) ? (object)DBNull.Value : sDevDtrFgtResultReport.modelNo.Trim().ToUpper()),
                new SqlParameter("@ModelName",String.IsNullOrEmpty(sDevDtrFgtResultReport.modelName) ? (object)DBNull.Value : sDevDtrFgtResultReport.modelName.Trim().ToUpper()),
                new SqlParameter("@CwaDateS",String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateS) ? (object)DBNull.Value : sDevDtrFgtResultReport.cwaDateS ),
                new SqlParameter("@CwaDateE",String.IsNullOrEmpty(sDevDtrFgtResultReport.cwaDateE) ? (object)DBNull.Value : sDevDtrFgtResultReport.cwaDateE ),
            };
            var data = new List<DevDtrFgtResultDto>();
            if(sDevDtrFgtResultReport.reportType =="Dev"){              //DEV
                pc.Add(new SqlParameter("@Season", String.IsNullOrEmpty(sDevDtrFgtResultReport.devSeason) ? (object)DBNull.Value : sDevDtrFgtResultReport.devSeason.Trim().ToUpper() ));
                pc.Add(new SqlParameter("@Factory",  sDevDtrFgtResultReport.factory.Trim().ToUpper()) );

                 data = await _context.GetDevDtrFgtResultDto
                   .FromSqlRaw("EXECUTE dbo.GetDTR_FGT_Result_Report_DEV @Article,@ModelNo,@ModelName,@CwaDateS,@CwaDateE,@Season,@Factory", pc.ToArray())
                   .ToListAsync();
            } else if(sDevDtrFgtResultReport.reportType =="Buy Plan"){
                pc.Add(new SqlParameter("@Season",  sDevDtrFgtResultReport.buyPlanSeason.Trim().ToUpper()) );
                pc.Add(new SqlParameter("@Factory",  sDevDtrFgtResultReport.factory.Trim().ToUpper()) );
                pc.Add(new SqlParameter("@VerNo",  (object)DBNull.Value) ); //because sp will find the biggest version of buy plan
                
                 data = await _context.GetDevDtrFgtResultDto
                   .FromSqlRaw("EXECUTE dbo.GetDTR_FGT_Result_Report_BuyPlan @Article,@ModelNo,@ModelName,@CwaDateS,@CwaDateE,@Season,@Factory,@VerNo", pc.ToArray())
                   .ToListAsync();            
            }            
            return data;

        }

        public async Task<List<BasicCodeDto>> GetBasicCodeDto(string typeNo)
        {
            string strWhere = " WHERE t1.DISABCODE = 1 ";   //禁用碼
            if (!(String.IsNullOrEmpty(typeNo)))
                strWhere += " AND t1.FKBASEFID IN (select PKBASEHID from BASEIDH where TYPENO = N'" + typeNo.Trim() + "' ) " ;
                                                              
            string strSQL = string.Format(@"
SELECT 
	t1.VALUE as [Param],
	t1.TYPENO as [Key],
	t2.BASENAME as ValueZh,
	t3.BASENAME as ValueEn,
	t2.MEMO1    as MemoZh1,
	t2.MEMO2    as MemoZh2,
	t2.MEMO3    as MemoZh3,
	t2.MEMO4    as MemoZh4,
	t3.MEMO1    as MemoEn1,
	t3.MEMO2    as MemoEn2,
	t3.MEMO3    as MemoEn3,
	t3.MEMO4    as MemoEn4
FROM BASEIDH t1
INNER JOIN BASEIDB t2
ON t1.PKBASEHID=t2.FKBASEHID and t2.LANGID='950'
INNER JOIN BASEIDB t3
ON t1.PKBASEHID=t3.FKBASEHID and t3.LANGID='437' ");
            strSQL += strWhere;
            var data = await _context.GetBasicCodeDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }

        public void  GetTransferToDTR(string factoryIdFrom,string factoryIdTo,string article)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FactoryIdFrom", factoryIdFrom),
                new SqlParameter("@FactoryIdTo", factoryIdTo),
                new SqlParameter("@Article", article),
            };
            var data =  _context.Database.ExecuteSqlRaw("EXEC GetTransferToDTR @FactoryIdFrom,@FactoryIdTo,@Article", pc.ToArray());

        }
        public async Task<List<SampleTrackReportDto>> GetSampleTrackDto()
        {
            var data = await _context.GetSampleTrackReportDto
                   .FromSqlRaw("EXECUTE dbo.GetSampleTrackReport")
                   .ToListAsync();
            return data;

        }
        public async Task<List<NoneDto>> DoSsbDtrVsFileUpdate(string factoryId, string season, string article )
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FACTORYID",factoryId ),
                new SqlParameter("@SEASON",season ),
                new SqlParameter("@ARTICLE",article )
            };
            var data = await _context.GetNoneDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.SSB_DTR_VSFILE_UPDATE @FACTORYID,@SEASON,@ARTICLE"), pc.ToArray())
                   .ToListAsync();
            return data;
        }
        public async Task<List<KanbanDataByLineDto>> GetKanbanDataByLineDto(string lineId )
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@LINEID",lineId )
            };
            var data = await _context.GetKanbanDataByLineDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.PROD_GET_KANBAN_PRODUCT_LINE @LINEID"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 
        public async Task<List<KanbanTQCDto>> GetKanbanTQCDto(string lineId)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@LINEID",lineId )
            };
            var data = await _context.GetKanbanTQCDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.PROD_GET_KANBAN_TQC @LINEID"), pc.ToArray())
                   .ToListAsync();
            return data;
        }

        public async Task<List<UserRoleDto>> GetUsersByName(string userName)
        {
            string strWhere =  String.Format(@"WHERE A.LOGIN='{0}'", userName);
            string strSQL = string.Format(@"
                                            SELECT  
                                                   A.WORKPNO
                                                  ,CONVERT(varchar,A.USERID) AS USERID
                                                  ,A.LOGIN
                                                  ,A.FACTORYID
                                            	  ,B.GROUPNO
                                                  ,C.EMAIL
                                              FROM STACCRTH AS A
                                              LEFT JOIN UTL_PJOBINER AS B ON A.USERID = B.USERID
                                              INNER JOIN SSBDEV3..UTL_USERINFO AS C ON A.USERID = C.USERID ");
            strSQL += strWhere;
            var data = await _context.UserRoleDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
        public async Task<List<DtrF206BomDto>> GetDtrF206Bom(string article )
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@ARTICLE",article ),
                new SqlParameter("@CR2","" ),
                new SqlParameter("@SMS","" ),
                new SqlParameter("@MCS","" ),
                new SqlParameter("@OTH","" ),
                new SqlParameter("@STAGE","" )                
            };
            var data = await _context.GetDtrF206BomDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.GetDtrF206Bom @ARTICLE,@CR2,@SMS,@MCS,@OTH,@STAGE"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 
        public async Task<List<SrfChangeDto>> GetSrfChange(string beforeDate )
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@BEFOREDATE",beforeDate ),
                new SqlParameter("@MODELNO","" )              
            };
            var data = await _context.GetSrfChangeDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.SSB_GET_SRF_CHANGE @BEFOREDATE,@MODELNO"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 
        public async Task<List<SrfDifferenceDto>> GetSrfDifference(string srfId1,string srfId2)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SRFID1",srfId1 ),
                new SqlParameter("@SRFID2",srfId2 )              
            };
            var data = await _context.GetSrfDifferenceDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.SSB_GET_SRF_DIFFERENCE @SRFID1,@SRFID2"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 
        public async Task<List<PrdEntryAccessDto>> GetPrdEntryAccessDto(string area,string recordTime)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Area",area ),
                new SqlParameter("@RecordTime",recordTime )              
            };
            var data = await _context.GetPrdEntryAccessDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.PrdEntryAccessApi @Area,@RecordTime"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 
        public async Task<List<PrdRfidAlertDto>> GetPrdRfidAlertDto(string recordTimeS,string recordTimeE)
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@RecordTimeS",recordTimeS ),
                new SqlParameter("@RecordTimeE",recordTimeE )               
            };
            var data = await _context.GetPrdRfidAlertDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.PrdRfidAlertApi @RecordTimeS,@RecordTimeE"), pc.ToArray())
                   .ToListAsync();
            return data;
        } 

                         
    }
   
}