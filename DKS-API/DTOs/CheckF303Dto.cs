using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class CheckF303Dto
    {
        public string? SampleNo { get; set; }
        public string? ReqSize { get; set; }
        public string? ShoeSize { get; set; }
        public Decimal? OrderPairs { get; set; }
        public Decimal? Sacle { get; set; }

    }
}