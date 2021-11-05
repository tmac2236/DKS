using System.Collections.Generic;
using System.Linq;
using DKS.API.Models.DKS;
using DKS_API.DTOs;

namespace DKS_API.Data.Interface
{
    public interface IDtrLoginHistoryDAO : ICommonDAO<DtrLoginHistory>
    {

         IQueryable<DtrLoginUserHistoryDto> GetDtrLoginUserHistoryDto(SDtrLoginHistory sDtrLoginHistory);

    }
}