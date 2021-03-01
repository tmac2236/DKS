using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SF340Schedule : PaginationParams
    {
        public string season { get; set; }
        public string bpVer { get; set; }
        public string cwaDate { get; set; }

    }
}