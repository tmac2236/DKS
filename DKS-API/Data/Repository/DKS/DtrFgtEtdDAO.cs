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
using DKS_API.Data;

namespace DFPS.API.Data.Repository
{
    public class DtrFgtEtdDAO: DKSCommonDAO<DtrFgtEtd>, IDtrFgtEtdDAO
    {
        public DtrFgtEtdDAO(DKSContext context) : base(context)
        {
        }
        public async Task<List< DtrFgtEtdDto>> GetDtrFgtEtdDto()
        {                                                  
            string strSQL = string.Format(@"
SELECT t1.FACTORYID AS Factoryid 
      ,t1.ARTICLE as Article 
      ,t1.STAGE as Stage 
      ,t1.TEST as Test 
      ,t1.QC_RECEIVE as QcReceive 
      ,t1.QC_ETD as QcEtd
      ,t1.REMARK as Remark
      ,t1.UPUSR as EtdUser
      ,t1.UPDAY as EtdDate
	  ,t2.LABNO as LabNo
      ,t2.UPUSR as FgtUser
	  ,t2.UPDAY as FgtDate
FROM DTR_FGT_ETD t1
left join DTR_FGT_RESULT t2
on  t1.FACTORYID=t2.FACTORYID and t1.ARTICLE=t2.ARTICLE and t1.TEST=t2.KIND
where (t1.STAGE in ('CR2','SMS','CS1') and t2.STAGE is null ) or
      (t1.STAGE='SMS' and t2.STAGE not in ('SMS','CS1')) or
      (t1.STAGE='CS1' and t2.STAGE not in ('CS1'))
order by QC_ETD ");

            var data = await _context.GetDtrFgtEtdDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
        public async Task<List<NoneDto>> DoSsbDtrQcEtdUpdate(DtrFgtEtdDto dto )
        {

            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@FACTORYID",dto.FactoryId ),
                new SqlParameter("@ARTICLE",dto.Article ),
                new SqlParameter("@TEST",dto.Test ),
                new SqlParameter("@STAGE",dto.Stage ),
                new SqlParameter("@QC_RECEIVE",dto.QcReceive ),
                new SqlParameter("@QC_ETD",dto.QcEtd ),
                new SqlParameter("@QC_REMARK",dto.Remark )
            };
            var data = await _context.GetNoneDto
                   .FromSqlRaw(string.Format("EXECUTE dbo.SSB_DTR_QCETD_UPDATE @FACTORYID,@ARTICLE,@TEST,@STAGE,@QC_RECEIVE,@QC_ETD,@QC_REMARK"), pc.ToArray())
                   .ToListAsync();
            return data;
        }        
    }
}