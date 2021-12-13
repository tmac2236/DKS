using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevPlmPart : PaginationParams
    {
        public string partno { get; set; }
        public string partnamecn { get; set; }
        public string partnameen { get; set; }
        public string location { get; set; }
    }
}