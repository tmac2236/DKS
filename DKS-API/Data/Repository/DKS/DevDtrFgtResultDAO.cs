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
    public class DevDtrFgtResultDAO : DKSCommonDAO<DevDtrFgtResult>, IDevDtrFgtResultDAO
    {
        public DevDtrFgtResultDAO(DKSContext context) : base(context)
        {
        }

        public async Task<List<DevDtrFgtResult>> GetDevDtrFgtResultByModelArticle(string modelNo, string article)
        {
                if(String.IsNullOrEmpty(modelNo))modelNo = "";
                if(String.IsNullOrEmpty(article))article = "";
                
                var data = await _context.DTR_FGT_RESULT
                .Where(x =>x.MODELNO.Contains(modelNo) && x.ARTICLE.Contains(article)).ToListAsync();

                return data;
        }
        public async Task<List<F340PartNoTreatmemtDto>> GetPartName4DtrFgt(string article,string stage)
        {
            string strWhere = " WHERE T1.TREATMENTCODE<> '' AND T1.RELEASE_LOGIN<>'' " ;//--已放行(T1.RELEASE_LOGIN<>'')
            if (!(String.IsNullOrEmpty(article))) strWhere += " AND T1.ARTICLE = '" + article.Trim() +"' " ;
            if (!(String.IsNullOrEmpty(stage))) {
                if(stage.Trim() == "MCS") stage = "CWA";    // because the CWA of DKS = the MSC offgt
                strWhere += " AND T1.RELEASE_TYPE = '" + stage.Trim() +"' " ;
            }
            string strSQL = string.Format(@"
SELECT T1.PARTNO as PartNo,
	   T1.PARTNAME as PartName,
	   T2.TreatmentCode,
	   T2.TreatmentZh,
	   T2.TreatmentEn
  FROM DEV_TREATMENT as T1
  left join (
			select 
				 (select RTRIM(MEMO1) from BASEIDB   where FKBASEHID=t1.PKBASEHID and LANGID='437') as TreatmentCode
				 ,RTRIM( t1.TYPENO) as [Code]
				 ,(select RTRIM(BASENAME) from BASEIDB where FKBASEHID=t1.PKBASEHID and LANGID='950') as TreatmentZh
				 ,(select RTRIM(BASENAME) from BASEIDB where FKBASEHID=t1.PKBASEHID and LANGID='437') as TreatmentEn
				 ,t1.DISABCODE
			  from BASEIDH t1
			  where t1.FKBASEFID='ABH000000959' and DISABCODE=1  --043
			  	) as T2
	ON T1.TREATMENTCODE = T2.TREATMENTCODE ");
            strSQL += strWhere;
            var data = await _context.GetF340PartNoTreatmemtDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
    }
}