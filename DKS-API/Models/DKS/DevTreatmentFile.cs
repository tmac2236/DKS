using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevTreatmentFile
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string ARTICLE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string PARTNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string TREATMENTCODE { get; set; }

        [Column(Order = 3)]
        [StringLength(1)]
        public string KIND { get; set; }

        [Column(Order = 4)]
        [StringLength(40)]
        public string FILE_NAME { get; set; }

        [Column(Order = 5)]
        [StringLength(300)]
        public string FILE_COMMENT { get; set; }

        [Column(Order = 6)]
        [StringLength(20)]
        public string UPUSR { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime UPTIME { get; set; }

    }
}