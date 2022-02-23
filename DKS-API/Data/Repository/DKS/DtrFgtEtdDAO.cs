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
on t1.FACTORYID=t2.FACTORYID and t1.ARTICLE=t2.ARTICLE and t1.STAGE=t2.STAGE and t1.TEST=t2.KIND
where LabNo is null
order by QC_ETD ");

            var data = await _context.GetDtrFgtEtdDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
    }
}