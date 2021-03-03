using System.Collections.Generic;
using System.Threading.Tasks;
using DKS.API.Models.DKS;
using DKS_API.DTOs;
using DKS_API.Helpers;

namespace DKS_API.Data.Interface
{
    public interface IWarehouseDAO
    {
        Task<bool> SaveAll();
        PagedList<F428SampleNoDetail> GetMaterialNoBySampleNoForWarehouse(SF428SampleNoDetail sF428SampleNoDetail);
        Task<List<F428SampleNoDetail>> GetMaterialNoBySampleNoForWarehouse4Excel(SF428SampleNoDetail sF428SampleNoDetail);
    }
}