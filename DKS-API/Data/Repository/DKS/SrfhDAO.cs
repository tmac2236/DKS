using DKS.API.Models.DKS;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DFPS.API.Data.Repository
{
    public class SrfhDAO: DKSCommonDAO<Srfh>, ISrfhDAO
    {
        public SrfhDAO(DKSContext context) : base(context)
        {
        }
        public List<SrfArticleDto> GetSrfArticleDto(string srfId)
        {
            var q =  from t1 in _context.SRFH
                        join t2 in _context.SRFARTIB on t1.SRFID equals t2.SRFID
                        where t1.SRFID == srfId
                       select new SrfArticleDto
                       {
                           SrfId = t1.SRFID,
                           ModelNo = t1.MODELNO,
                           Stage = t1.SAMPURSRF,
                           Article = t2.ARTICLE
                       };

            return q.ToList();
        }            
    }
}