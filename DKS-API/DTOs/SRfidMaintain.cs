using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SRfidMaintain : PaginationParams
    {
        public string gate { get; set; }
        public string time { get; set; }
        public string epc { get; set; }
        public string seq { get; set; }
    }
}