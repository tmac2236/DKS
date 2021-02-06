using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class Articled
    {
        [Required]
        [StringLength(6)]
        public string ARTICLE { get; set; }

        public DateTime? CWADEADL { get; set; }

        [Required]
        [StringLength(10)]
        public string CWID { get; set; }

        [Required]
        [StringLength(40)]
        public string CWNAME { get; set; }

        [Required]
        [StringLength(15)]
        public string MODELNO { get; set; }

        public byte[] PICTURE { get; set; }

        [Key]
        [StringLength(12)]
        public string PKARTBID { get; set; }

        [Required]
        [StringLength(20)]
        public string PROBRIID { get; set; }

        [Required]
        [StringLength(8)]
        public string PROSTATUSID { get; set; }

        [StringLength(200)]
        public string REMARK { get; set; }

        [StringLength(20)]
        public string REQSIZE { get; set; }

        public DateTime? RIDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SIGNTRACK { get; set; }

        [Required]
        [StringLength(3)]
        public string STAGE { get; set; }

        [StringLength(30)]
        public string TESTLEVEL { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MKUSERID { get; set; }

        public DateTime INSERDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }

        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }
    }
}