using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DtrFgtShoes
    {
        public string? FACTORYID { get; set; }
        [Key]
        public string SAMPLENO { get; set; }
        public string ARTICLE { get; set; }
        public string STAGE { get; set; }
        public string TEST { get; set; }
        public DateTime QC_RECEIVE { get; set; }
        public DateTime? QC_ETD { get; set; }
    }
}