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

namespace DFPS.API.Data.Repository
{
    public class WarehouseDAO : IWarehouseDAO
    {
        private readonly DKSContext _context;
        public WarehouseDAO(DKSContext context)
        {
            _context = context;
        }

        public PagedList<F428SampleNoDetail> GetMaterialNoBySampleNoForWarehouse(SF428SampleNoDetail sF428SampleNoDetail)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SampleNo",sF428SampleNoDetail.SampleNo.Trim().ToUpper())
            };

            var data =  _context.GetMaterialNoBySampleNoForWarehouseView
                   .FromSqlRaw("EXECUTE dbo.GetMaterialNoBySampleNoForWarehouse @SampleNo", pc.ToArray())
                   .ToList();

                 return PagedList<F428SampleNoDetail>
                .Create(data, sF428SampleNoDetail.PageNumber, sF428SampleNoDetail.PageSize, sF428SampleNoDetail.IsPaging);
        }

        public Task<List<F428SampleNoDetail>> GetMaterialNoBySampleNoForWarehouse4Excel(SF428SampleNoDetail sMaterialDetailBySampleNo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAll()
        {
            throw new NotImplementedException();
        }
    }
}