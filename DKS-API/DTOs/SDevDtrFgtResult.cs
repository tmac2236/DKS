using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevDtrFgtResult : PaginationParams
    {
        public string article { get; set; }
        public string modelNo { get; set; }
        public string modelName { get; set; }
        
    }
}