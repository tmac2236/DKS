using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SRfidMaintain : PaginationParams
    {
        public string recordTimeS { get; set; }
        public string recordTimeE { get; set; }
        public string area { get; set; }
    }
}