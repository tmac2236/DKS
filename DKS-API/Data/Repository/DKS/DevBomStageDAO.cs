using DKS.API.Models.DKS;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;


namespace DFPS.API.Data.Repository
{
    public class DevBomStageDAO: DKSCommonDAO<DevBomStage>, IDevBomStageDAO
    {
        public DevBomStageDAO(DKSContext context) : base(context)
        {
        }

    }
}