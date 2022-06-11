using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class F434Dto
    {
        public string? SsbMatPid { get; set; }
        public string? StockNo { get; set; }
        public string? WareCode { get; set; }
        public string? Location { get; set; }
        public string? MaterialNo { get; set; }

        public string? MaterialName { get; set; }
        public string Color { get; set; }
        public string? ShoeSize { get; set; }
        public string? Unit { get; set; }
        public Decimal? MaterialQty { get; set; }
        
        public string? Season { get; set; }
        public string? Stage { get; set; }
        public string? OrderStage { get; set; }
        public string? DevTeam { get; set; }
        public string? ModelNameMemo { get; set; }

        public string? Article { get; set; }
        public string? FmcaTestResult { get; set; }
        public string SingleTestResult { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? Memo { get; set; }

        public string? OrderNumber { get; set; }

    }
}