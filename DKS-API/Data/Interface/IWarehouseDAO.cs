using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Helpers;

namespace DKS_API.Data.Interface
{
    public interface IWarehouseDAO
    {
        PagedList<F428SampleNoDetail> GetMaterialNoBySampleNoForWarehouse(SF428SampleNoDetail sF428SampleNoDetail);
        Task<List<F428SampleNoDetail>> GetMaterialNoBySampleNoForWarehouse4Excel(SF428SampleNoDetail sF428SampleNoDetail);
        Task<List<StockDetailByMaterialNo>> GetStockDetailByMaterialNo(SF428SampleNoDetail sF428SampleNoDetail);
        Task<List<F406iDto>> GetF406iDto(SF406i sF406iDto);
        Task<List<P406Dto>> GetP406Dto(string acpDateS,string acpDateE);
        Task<List<F434Dto>> GetF434Dto(SF406i sF406iDto);
        Task<List<CheckF303Dto>> GetCheckF303Dto(string sampleNo);
        Task<List<GetF303MatQtyDto>> GetF303MatQtyDto(string sampleNo);
        Task<List<GetF303PartQtyDto>> GetF303PartQtyDto(string sampleNo);
    }
}