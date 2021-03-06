using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SF428SampleNoDetail : PaginationParams
    {
        public string SampleNo { get; set; }
        public string MaterialNo { get; set; }

    }
}