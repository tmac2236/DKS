using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevBuyPlan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string SEASON { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string MODELNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string SCOLOR { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string ARTICLE { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short VERN { get; set; }

        [StringLength(1)]
        public string FLAG { get; set; }

        [StringLength(20)]
        public string SAMPLENO { get; set; }

        [StringLength(6)]
        public string UPUSR { get; set; }

        public DateTime? UPTIME { get; set; }
    }
}