using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class SamDetlB
    {
[Required]
        [StringLength(10)]
        public string MATERIANO { get; set; }

        public string PARTNAME2 { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string PKSAMDLID { get; set; }

        [StringLength(4)]
        public string PROCESS { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string SAMPLENO { get; set; }

        [StringLength(60)]
        public string SHESIZE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MATCONS { get; set; }

        [StringLength(1)]
        public string GEMATERIA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal WMATCONS { get; set; }
    }
}