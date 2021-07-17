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
    }
}