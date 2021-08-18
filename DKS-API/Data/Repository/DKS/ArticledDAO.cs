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
    }
}