using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    ///F420-加工驗收單B
    public class Pracdatb
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string PKPRACBID { get; set; }

        [StringLength(8)]
        public string PAYYM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PAYMENTS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PAYPRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRPAYQTY { get; set; }

        [Required]
        [StringLength(15)]
        public string PROORDNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string PROACCNO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRACQTY { get; set; }

        [StringLength(200)]
        public string MEMO { get; set; }
    }
}