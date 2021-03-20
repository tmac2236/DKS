using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SF340Schedule : PaginationParams
    {
        public string dataType { get; set; }
        public string factory { get; set; }
        public string season { get; set; }
        public string bpVer { get; set; }
        public string cwaDateS { get; set; }
        public string cwaDateE { get; set; }
    }
}