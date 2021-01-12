using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFPS.API.Data.Interface;
using DFPS.API.Models;
using DFPS.API.Models.DKSSys;
using DFPS_API.Data.Interface;
using DFPS_API.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DFPS.API.Data.Repository
{
    public class DKSDAO : IDKSDAO
    {
        private readonly DKSContext _context;
        public DKSDAO(DKSContext context)
        {
            _context = context;
        }
        public async Task<List<Ordsumoh>> SearchConvergence(string season, string stage)
        {
            var list = await _context.ORDSUMOH.Where(x => x.SEASON == season && x.STAGE == stage).OrderBy(x=>x.PRSUMNO).ToListAsync();
            return list;
        }
    }
}