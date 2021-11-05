using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
public  class EmpDataH
    {
        [StringLength(80)]
        public string EMAIL { get; set; }

        [Key]
        [StringLength(5)]
        public string WORKPNO { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        public DateTime DUTYDAY { get; set; }

        [Required]
        [StringLength(20)]
        public string NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MKUSERID { get; set; }

        public DateTime INSERDATE { get; set; }

        [Required]
        [StringLength(1)]
        public string MASTER { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }

        [Required]
        [StringLength(5)]
        public string DEPTID { get; set; }

        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }

        public DateTime? OUTDATE { get; set; }
    }
}