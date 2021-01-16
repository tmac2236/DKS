using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class Staccrth
    {
        [Key]
        [StringLength(12)]
        public string PKPLGHID { get; set; }

        [Required]
        [StringLength(5)]
        public string WORKPNO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CHKSUM { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal USERID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MKUSERID { get; set; }

        public DateTime INSERDATE { get; set; }

        [StringLength(50)]
        public string LOGIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }

        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }
    }
}