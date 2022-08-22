using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class GetF303MatQtyDto
    {
        public string? SampleNo { get; set; }
        public string? MaterialNo { get; set; }
        public string? ShoeSize { get; set; }
        public Decimal? MatCons { get; set; }
        public Decimal? TtlPair { get; set; }
        public string? Uom { get; set; }
        public Decimal? MatConsWeighted { get; set; }

    }
}