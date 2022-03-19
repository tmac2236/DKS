using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SP202 : PaginationParams
    {
        public string season { get; set; }
        public string brand{ get; set; }
        public string modelName{ get; set; }
        public string modelNo{ get; set; }
        public string article{ get; set; }
    }
}