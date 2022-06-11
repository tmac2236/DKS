using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SF406i : PaginationParams
    {
        public string StockNo { get; set; }
        public string  MaterialNo { get; set; }
        public int Type { get; set; }
    }
}