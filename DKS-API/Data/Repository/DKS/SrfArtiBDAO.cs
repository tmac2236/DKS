using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS.API.Models.DKS;

namespace DFPS.API.Data.Repository
{
    public class SrfArtiBDAO: DKSCommonDAO<SrfArtiB>, ISrfArtiBDAO
    {
        public SrfArtiBDAO(DKSContext context) : base(context)
        {
        }

    }
}