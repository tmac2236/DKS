using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SF340PPDSchedule : PaginationParams
    {
        public string factory { get; set; }
        public string season { get; set; }
        public string bpVer { get; set; }
        public string cwaDateS { get; set; }
        public string cwaDateE { get; set; }
        public string article { get; set; }
        public string modelNo { get; set; }
        public string modelName { get; set; }


    }
}