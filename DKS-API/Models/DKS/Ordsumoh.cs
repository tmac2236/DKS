using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class Ordsumoh
    {
        public string? BRANDCATE { get; set; }
        public string SEASON { get; set; }
        public string STAGE { get; set; }
        public string ETPID { get; set; }
        public string STATUS { get; set; }

        public string BRANDNO { get; set; }
        [Column(TypeName = "decimal(9,0)")]
        public decimal MKUSERID { get; set; }
        public DateTime INSERDATE { get; set; }
        [Key]
        public string PRSUMNO { get; set; }
        [Column(TypeName = "decimal(9,0)")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }
        public DateTime SUMDATE { get; set; }
        public string SUMMTYPE { get; set; }
        public DateTime? PASSDATE { get; set; }
        public string FACTORYID { get; set; }
    }
}