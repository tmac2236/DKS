using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class SamPartB
    {
        [Column(TypeName = "numeric")]
        public decimal? CONS { get; set; }

        [Required]
        [StringLength(10)]
        public string MATERIANO { get; set; }

        [Required]
        [StringLength(10)]
        public string ORIGINNO { get; set; }

        [StringLength(60)]
        public string PARTNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string PARTNO { get; set; }

        [StringLength(4)]
        public string PROCESS { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string SAMPLENO { get; set; }

        [StringLength(60)]
        public string SHESIZE { get; set; }

        public DateTime? MDFDATE { get; set; }

        [StringLength(1)]
        public string GEMATERIA { get; set; }

        [StringLength(255)]
        public string MATERNAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PARTQTY { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [StringLength(200)]
        public string CHKSTOCKNO { get; set; }

        [StringLength(100)]
        public string CHKUSR { get; set; }

        public DateTime? CHKTIME { get; set; }
    }
}