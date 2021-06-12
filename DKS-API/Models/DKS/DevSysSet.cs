using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevSysSet
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYSKEY { get; set; }

        [Column(Order = 1)]
        [StringLength(100)]
        public string SYSVAL { get; set; }


        [Column(Order = 2)]
        [StringLength(20)]
        public string UPUSR { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime UPTIME { get; set; }

    }
}