using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDtrLoginHistory : PaginationParams
    {
        public string systemName { get; set; }
        public string factoryId { get; set; }
        public string loginTimeS { get; set; }
        public string loginTimeE { get; set; }

    }
}