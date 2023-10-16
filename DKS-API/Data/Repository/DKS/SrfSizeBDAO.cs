using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS.API.Models.DKS;

namespace DFPS.API.Data.Repository
{
    public class SrfSizeBDAO: DKSCommonDAO<SrfSizeB>, ISrfSizeBDAO
    {
        public SrfSizeBDAO(DKSContext context) : base(context)
        {
        }

    }
}