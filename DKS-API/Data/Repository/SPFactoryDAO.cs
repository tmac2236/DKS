using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DFPS.API.Data.Interface;
using DFPS.API.DTOs;
using DFPS_API.DTOs;
using System;
using DFPS_API.Helpers;

namespace DFPS_API.Data.Repository
{
    public class SPFactoryDAO : ISPFactoryDAO
    {
        private readonly DataContext _context;
        public SPFactoryDAO(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectLean>> GetAllLeans()
        {
            var data = await _context.GetAllLeanId
                        .FromSqlRaw("EXECUTE dbo.SSB_GET_LINE_ID")
              .ToListAsync();
            data = data.OrderBy(x => x.IOrder).ToList();
            return data;
        }

        public async Task<IEnumerable<SelectModelByLean>> GetModelsByLean(string lean)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@Maker",lean)
            };
            var data = await _context.GetAllModelByLean
            .FromSqlRaw("EXECUTE dbo.SSB_GET_LINE_MODEL @Maker", pc.ToArray())
            .ToListAsync();
            return data;
        }
    }
}