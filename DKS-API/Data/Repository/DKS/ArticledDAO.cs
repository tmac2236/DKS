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
        public async Task<List<ArticleModelNameDto>> GetArticleModelNameDto(string modelNo,string article)
        {
            string strWhere = " WHERE 1=1 ";
            if (!(String.IsNullOrEmpty(article)))
                strWhere += " AND t1.ARTICLE = N'" + article.Trim()  + "' " ;
            if (!(String.IsNullOrEmpty(modelNo)))
                strWhere += " AND t1.MODELNO like N'" + modelNo.Trim()  + "%' " ;
            string strSQL = string.Format(@"
SELECT 
       t1.ARTICLE   as Article
      ,t1.MODELNO   as ModelNo 
      ,t1.STAGE	    as Stage
      ,t1.[STATUS]  as [Status]
	  ,t2.MODELNAME as ModelName

  FROM ARTICLED as t1
  LEFT JOIN MODELDAH as t2 on t1.MODELNO = t2.MODELNO ");
            strSQL += strWhere;
            var data = await _context.GetArticleModelNameDto.FromSqlRaw(strSQL).ToListAsync();
            return data;
        }
    }
}