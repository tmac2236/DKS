using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class F406iDto
    {
        public string? SheSize { get; set; }
        public string? OrderNo { get; set; }
        public string? Location { get; set; }
        public string? StockNo { get; set; }
        public string? MaterialName { get; set; }

        public string? Unit { get; set; }
        public string MaterialNo { get; set; }
        public Decimal? AccQty { get; set; }
        public DateTime? AcptDate { get; set; }
        public string? ColorTrea { get; set; }
        
        public string? SsbMatPid { get; set; }
        public string? Thickness { get; set; }
        public string? Fmca1 { get; set; }
        public string? ColorCode { get; set; }
        public string? SupName { get; set; }

        public string? TestYn { get; set; }
        public string? ArticleMemo { get; set; }
        public string ModelMemo { get; set; }
        public string? ODSTAGE20 { get; set; }
        public string? DevTeamName { get; set; }

        public string? Season { get; set; }

    }
}