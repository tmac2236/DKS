using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DKS.API.Models.DKS
{
    //F418 加工採購單H
    public class Proporh
    {
        [StringLength(6)]
        public string ARTICLE { get; set; }

        [StringLength(1)]
        public string BRANDCATE { get; set; }

        [StringLength(40)]
        public string MODELNAME { get; set; }

        [StringLength(15)]
        public string MODELNO { get; set; }

        [StringLength(3)]
        public string ORDERSTAG { get; set; }

        [Required]
        [StringLength(30)]
        public string SAMPLENO { get; set; }

        [StringLength(4)]
        public string SEASON { get; set; }

        public string PRMANAME { get; set; }

        public string PROMATNM { get; set; }

        public DateTime PRORDDATE { get; set; }

        [Key]
        [StringLength(15)]
        public string PROORDNO { get; set; }

        [StringLength(15)]
        public string PORORDSNO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PROORDQTY { get; set; }

        [StringLength(100)]
        public string PROPART { get; set; }

        [Required]
        [StringLength(8)]
        public string PUNITCODE { get; set; }

        [Required]
        [StringLength(4)]
        public string PROSUPID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PROQTY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRACQTY { get; set; }

        [Required]
        [StringLength(1)]
        public string ETPID { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MKUSERID { get; set; }

        public DateTime INSERDATE { get; set; }

        [StringLength(20)]
        public string STITCHTM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MDUSERID { get; set; }

        public DateTime? CHANGDATE { get; set; }

        [StringLength(200)]
        public string MEMO { get; set; }

        [StringLength(100)]
        public string OUTBOUQTY { get; set; }

        [StringLength(20)]
        public string FITBMETHD { get; set; }

        [StringLength(60)]
        public string DEVTEMNA { get; set; }

        public DateTime? EXFACDATE { get; set; }

        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }
    }
}