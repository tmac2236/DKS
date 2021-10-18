using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevDtrFgtResult
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ARTICLE { get; set; }

        [Key]
        [StringLength(10)]
        public string STAGE { get; set; }

        [Key]
        [StringLength(10)]
        public string KIND { get; set; }

        [Required]
        [StringLength(10)]
        public string TYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MODELNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string MODELNAME { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string LABNO { get; set; }

        [Required]
        [StringLength(10)]
        public string RESULT { get; set; }

        [Required]
        [StringLength(10)]
        public string PARTNO { get; set; }

        [Required]
        [StringLength(200)]
        public string PARTNAME { get; set; }

        [Required]
        [StringLength(100)]
        public string FILENAME { get; set; }
        [StringLength(500)]
        public string? REMARK { get; set; }

        public DateTime? UPDAY { get; set; }

        [Required]
        [StringLength(50)]
        public string UPUSR { get; set; }
    }
}