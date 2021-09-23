using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevDtrVsReport : PaginationParams
    {
        public string Season { get; set; }

        public string Article { get; set; }
        
        public string FactoryId { get; set; }
        
    }
}