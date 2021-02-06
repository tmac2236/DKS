using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class ModelDah
    {
       [StringLength(40)]
        public string BOTOOLING { get; set; }

        [StringLength(1)]
        public string BRANDCATE { get; set; }

        [Required]
        [StringLength(8)]
        public string CATEGORYID { get; set; }

        [Required]
        [StringLength(8)]
        public string CUSTOMERNO { get; set; }

        [Required]
        [StringLength(5)]
        public string DEVELOPERID { get; set; }

        [Required]
        [StringLength(8)]
        public string DEVTEAMID { get; set; }

        [Required]
        [StringLength(8)]
        public string GENDERID { get; set; }

        [StringLength(20)]
        public string LAST { get; set; }

        [StringLength(40)]
        public string MATWAYID { get; set; }

        [StringLength(40)]
        public string MAWANAME { get; set; }

        [StringLength(10)]
        public string MODFAMILY { get; set; }

        [StringLength(40)]
        public string MODELNAME { get; set; }

        [Key]
        [StringLength(15)]
        public string MODELNO { get; set; }

        [StringLength(2)]
        public string PRODTYPE { get; set; }

        [StringLength(200)]
        public string REMARK { get; set; }

        [Required]
        [StringLength(4)]
        public string SEASON { get; set; }

        [Required]
        [StringLength(40)]
        public string SILHOUETT { get; set; }

        [StringLength(20)]
        public string SIZERUN { get; set; }

        [Required]
        [StringLength(8)]
        public string SIZESCALEID { get; set; }

        [Required]
        [StringLength(8)]
        public string SIZETYPEID { get; set; }

        [StringLength(40)]
        public string UPTOOLING { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [Required]
        [StringLength(8)]
        public string BRANDNO { get; set; }

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