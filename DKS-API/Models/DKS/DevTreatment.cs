using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevTreatment
    {
        [Required]
        [StringLength(6)]
        public string ARTICLE { get; set; }

        public DateTime? B_COLORCARD { get; set; }

        [Required]
        [StringLength(1)]
        public string BIZ_FLAG { get; set; }

        public DateTime? BIZ_P_TIME { get; set; }

        [StringLength(1)]
        public string CATEGORY { get; set; }

        [StringLength(50)]
        public string CREATE_LOGIN { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        [StringLength(50)]
        public string DEV_LOGIN2 { get; set; }

        public DateTime? DEV_DATE { get; set; }

        public DateTime? DEV_DATE2 { get; set; }

        [StringLength(50)]
        public string DEV_LOGIN { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long GUID { get; set; }

        [StringLength(10)]
        public string HPMATERIANO { get; set; }

        [Required]
        [StringLength(10)]
        public string HPPARTNO { get; set; }

        [Required]
        [StringLength(10)]
        public string HPSUPID { get; set; }

        [Required]
        [StringLength(10)]
        public string HPSUPID2 { get; set; }

        [Required]
        [StringLength(255)]
        public string MATERNAME { get; set; }

        [Required]
        [StringLength(10)]
        public string MATERIANO { get; set; }

        [Required]
        [StringLength(60)]
        public string PARTNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string PARTNO { get; set; }

        [StringLength(8)]
        public string PARTNO_OLD { get; set; }

        public DateTime? PDM_DATE { get; set; }

        [StringLength(50)]
        public string PDM_LOGIN { get; set; }

        [StringLength(50)]
        public string RELEASE_LOGIN { get; set; }

        public DateTime? RELEASE_DATE { get; set; }

        [StringLength(3)]
        public string RELEASE_TYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string SAMPLENO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string TREATMENTCODE { get; set; }

        public DateTime? TT_DATE { get; set; }

        public DateTime? TT_DATE2 { get; set; }

        [StringLength(50)]
        public string TT_LOGIN { get; set; }

        [StringLength(50)]
        public string TT_LOGIN2 { get; set; }

        public DateTime? U_REALCARD { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "numeric")]
        public decimal VERNO { get; set; }

        public DateTime? WORKFLOW { get; set; }

        [StringLength(1)]
        public string WORKSHP { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public DateTime? MD_DATE { get; set; }

        [Required]
        [StringLength(30)]
        public string MD_LOGIN { get; set; }

        [StringLength(1)]
        public string EDIT_FLAG { get; set; }

        [Required]
        [StringLength(4)]
        public string SUPCODE { get; set; }

        [Required]
        [StringLength(4)]
        public string SUPCODE2 { get; set; }

        [Required]
        [StringLength(20)]
        public string SUPSNAME { get; set; }

        [Required]
        [StringLength(20)]
        public string SUPSNAME2 { get; set; }

        [StringLength(1)]
        public string SHOES { get; set; }

        [StringLength(500)]
        public string PPD_REMARK { get; set; }

        [StringLength(40)]
        public string PHOTO { get; set; }
        [StringLength(300)]
        public string PHOTO_COMMENT { get; set; }
        [StringLength(40)]
        public string PDF { get; set; }
        [Required]
        [StringLength(1)]
        public string FACTORYID { get; set; }
        [StringLength(10)]  //2022 04 10 Add
        public string STYLE { get; set; } 

    }
}