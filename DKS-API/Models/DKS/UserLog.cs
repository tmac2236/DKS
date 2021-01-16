using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class UserLog
    {
        [Required]
        [StringLength(10)]
        public string PROGNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string LOGINNAME { get; set; }

        [Required]
        [StringLength(250)]
        public string HISTORY { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime UPDATETIME { get; set; }
    }
}