using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class GetF303PartQtyDto
    {
        public string? SampleNo { get; set; }
        public string? PartNo { get; set; }
        public string? MaterialNo { get; set; }
        public string? ShoeSize { get; set; }
        public Decimal? Cons { get; set; }
        public Decimal? PartQty { get; set; }
        public Decimal? TtlPair { get; set; }
        public Decimal? PartConsWeighted { get; set; }

    }
}