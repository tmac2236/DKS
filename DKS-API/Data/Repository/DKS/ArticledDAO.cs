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
    public class ArticledDAO : DKSCommonDAO<Articled>, IArticledDAO
    {
        public ArticledDAO(DKSContext context) : base(context)
        {
        }
        public async Task<List<ArticleModelNameDto>> GetArticleModelNameDto(string modelNo,string article,string modelName)
        {
            string strWhere = " WHERE 1=1 ";
            if (!(String.IsNullOrEmpty(article)))
                strWhere += " AND t1.ARTICLE = N'" + article.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(modelNo)))
                strWhere += " AND t1.MODELNO like N'" + modelNo.Trim()  + "%' " ;
            if (!(String.IsNullOrEmpty(modelName)))
                strWhere += " AND t2.MODELNAME = N'" + modelName.Trim()  + "' " ;                
            string strSQL = string.Format(@"
SELECT DISTINCT
       t1.ARTICLE   as Article
      ,t1.MODELNO   as ModelNo 
	  ,t2.MODELNAME as ModelName
  FROM ARTICLED as t1
  LEFT JOIN MODELDAH as t2 on t1.MODELNO = t2.MODELNO ");
            strSQL += strWhere;
            var data = await _context.GetArticleModelNameDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }

        public async Task<List<ArticleSeasonDto>> GetArticleSeasonDto(string season, string article)
        {
            string strWhere = " WHERE 1=1 ";
            if (!(String.IsNullOrEmpty(article)))
                strWhere += " AND t1.ARTICLE = N'" + article.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(season)))
                strWhere += " AND t2.SEASON = N'" + season.Trim()  + "' " ;
             
            string strSQL = string.Format(@"
SELECT DISTINCT
       t2.SEASON    as Season
      ,t1.ARTICLE   as Article
      ,t1.MODELNO   as ModelNo 
	  ,t2.MODELNAME as ModelName
	  ,t2.DEVELOPERID as DeveloperId
	  ,t2.DEVTEAMID   as DevTeamId      
  FROM ARTICLED as t1
  LEFT JOIN MODELDAH as t2 on t1.MODELNO = t2.MODELNO
   ");
            strSQL += strWhere;
            var data = await _context.GetArticleSeasonDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }

        public async Task<List<DevDtrVsListDto>> GetDevDtrVsListDto(SDevDtrVsList sDevDtrVsList)
        {
            string strWhere = " WHERE 1=1 ";
            if (!(String.IsNullOrEmpty(sDevDtrVsList.Article)))
                strWhere += " AND t1.ARTICLE = N'" + sDevDtrVsList.Article.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(sDevDtrVsList.Season)))
                strWhere += " AND t2.SEASON = N'" + sDevDtrVsList.Season.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(sDevDtrVsList.ModelNo)))
                strWhere += " AND t1.MODELNO like N'" + sDevDtrVsList.ModelNo.Trim()  + "%' " ;
            if (!(String.IsNullOrEmpty(sDevDtrVsList.ModelName)))
                strWhere += " AND t2.MODELNAME = N'" + sDevDtrVsList.ModelName.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(sDevDtrVsList.DeveloperId)))
                strWhere += " AND t2.DEVELOPERID = N'" + sDevDtrVsList.DeveloperId.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(sDevDtrVsList.DevTeamId)))
                strWhere += " AND t2.DEVTEAMID = N'" + sDevDtrVsList.DevTeamId.Trim()  + "' " ;
                                                                            
            string strSQL = string.Format(@"
SELECT DISTINCT
       t2.SEASON    as Season
      ,t1.ARTICLE   as Article
      ,t1.MODELNO   as ModelNo 
	  ,t2.MODELNAME as ModelName
	  ,t3.LOGIN  as DeveloperId
	  ,t2.DEVTEAMID   as DevTeamId      
  FROM ARTICLED as t1
  LEFT JOIN MODELDAH as t2 on t1.MODELNO = t2.MODELNO 
  LEFT JOIN STACCRTH as t3 on t2.DEVELOPERID = t3.WORKPNO");
            strSQL += strWhere;
            var data = await _context.GetDevDtrVsListDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }

        public async Task<List<TupleDto>> GetSeasonNumDto()
        {
            //string strWhere = " WHERE 1=1 ";
            //if (!(String.IsNullOrEmpty(article)))
            //    strWhere += " AND t1.ARTICLE = N'" + article.Trim()  + "' " ;            
            string strSQL = string.Format(@"
SELECT
	DevSeason as [K]
	,Cast(count(DevSeason) as nvarchar) as [V]
	FROM(
	SELECT 
		t2.SEASON as DevSeason,
		t2.DEVTEAMID,
		t1.ARTICLE,
		t1.PROSTATUSID,
		t1.PKARTBID,
		isnull(convert(varchar,t1.ACTDATE,111),'1911/01/01') as CWADEADL,
		t1.MODELNO,
		t2.MODELNAME,
		t3.DEVSTATUS,--是否有轉單
		isnull(convert(varchar,t3.DROPDATE,111),'') as DROPDATE,
		ROW_NUMBER() OVER (PARTITION BY t1.ARTICLE ORDER BY t1.CHANGDATE DESC) AS id --給序號從1開始
	FROM ARTICLED t1
	inner join MODELDAH t2
	on	t2.BRANDCATE in ('1','2','3','6' ) --Brand Category = 1,2,3
		and  t1.MODELNO=t2.MODELNO
		inner join  RDBOMDAH t3
	on t1.PKARTBID=t3.FKARTICID 
		and t3.DEVSTATUS in (' ', '2') --排除DROP = 1 的Article
) as t1
WHERE t1.id = 1
GROUP BY DevSeason ");
            //strSQL += strWhere;
            var data = await _context.GetTupleDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
    }
}