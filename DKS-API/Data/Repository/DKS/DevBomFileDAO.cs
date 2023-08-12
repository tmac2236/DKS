using DKS.API.Models.DKS;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;


namespace DFPS.API.Data.Repository
{
    public class DevBomFileDAO: DKSCommonDAO<DevBomFile>, IDevBomFileDAO
    {
        public DevBomFileDAO(DKSContext context) : base(context)
        {
        }

    }
}