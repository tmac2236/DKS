using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using Microsoft.Data.SqlClient;
using System;
using DKS_API.Helpers;
using DKS.API.Models.DKS;

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

        public async Task<List<StockDetailByMaterialNo>> GetStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SampleNo",sF428SampleNoDetail.SampleNo.Trim()),
                new SqlParameter("@MaterialNo",sF428SampleNoDetail.MaterialNo.Trim())
            };

            var data = await  _context.GetStockDetailByMaterialNoView
                   .FromSqlRaw("EXECUTE dbo.GetStockDetailByMaterialNo @SampleNo,@MaterialNo", pc.ToArray())
                   .ToListAsync();
            return data;
        }
        public Task<List<F428SampleNoDetail>> GetMaterialNoBySampleNoForWarehouse4Excel(SF428SampleNoDetail sF428SampleNoDetail)
        {
            throw new NotImplementedException();
        }
        public async Task<List<F406iDto>> GetF406iDto(SF406i sF406iDto)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@StockNo",sF406iDto.StockNo)
            };

            var data = await  _context.GetF406iDto
                   .FromSqlRaw("EXECUTE dbo.GetF406I @StockNo", pc.ToArray())
                   .ToListAsync();
            return data;
        }

        public async Task<List<P406Dto>> GetP406Dto(string acpDateS, string acpDateE)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@AcpDateS",acpDateS.Trim()),
                new SqlParameter("@AcpDateE",acpDateE.Trim())
            };

            var data = await  _context.GetP406Dto
                   .FromSqlRaw("EXECUTE dbo.GetP406 @AcpDateS,@AcpDateE", pc.ToArray())
                   .ToListAsync();
            return data;
        }

        public async Task<List<F434Dto>> GetF434Dto(SF406i sF406iDto)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@MaterialNo",sF406iDto.MaterialNo)
            };

            var data = await  _context.GetF434Dto
                   .FromSqlRaw("EXECUTE dbo.GetF434Dto @MaterialNo", pc.ToArray())
                   .ToListAsync();
            return data;
        }
        public async Task<List<CheckF303Dto>> GetCheckF303Dto(string sampleNo)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SampleNo",sampleNo)
            };

            var data = await _context.GetCheckF303Dto
                   .FromSqlRaw(" EXECUTE dbo.CheckF303 @SampleNo ", pc.ToArray())
                   .ToListAsync();
            return data;
        }

        public async Task<List<GetF303MatQtyDto>> GetF303MatQtyDto(string sampleNo)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SampleNo",sampleNo)
            };

            var data = await _context.GetF303MatQtyDto
                   .FromSqlRaw(" EXECUTE dbo.GetF303_MATQTY @SampleNo ", pc.ToArray())
                   .ToListAsync();
            return data;
        }

        public async Task<List<GetF303PartQtyDto>> GetF303PartQtyDto(string sampleNo)
        {
            List<SqlParameter> pc = new List<SqlParameter>{
                new SqlParameter("@SampleNo",sampleNo)
            };

            var data = await _context.GetF303PartQtyDto
                   .FromSqlRaw(" EXECUTE dbo.GetF303_PARTQTY @SampleNo ", pc.ToArray())
                   .ToListAsync();
            return data;
        }
    }
}