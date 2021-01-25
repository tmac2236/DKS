using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    ///F420-加工驗收單H
    public class Pracdath
    {
        [Required]
        [StringLength(1)]
        public string CCMORDER { get; set; }

        [Required]
        [StringLength(4)]
        public string PROSUPID { get; set; }

        public DateTime PROACDATE { get; set; }

        [Key]
        [StringLength(15)]
        public string PROACCNO { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MKUSERID { get; set; }

        public DateTime INSERDATE { get; set; }

        [StringLength(30)]
        public string DELIVERNO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }

        [StringLength(200)]
        public string MEMO { get; set; }

        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }
    }
}