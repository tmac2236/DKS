using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevDtrFgtResultReport : PaginationParams
    {
        public string devSeason { get; set; }

        public string buyPlanSeason { get; set; }
        public string factory { get; set; }

        public string reportType { get; set; }
        public string article { get; set; }
        public string cwaDateS { get; set; }
        public string cwaDateE { get; set; }
        public string modelNo { get; set; }
        public string modelName { get; set; }

    }
}