using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevDtrFgtStats
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ARTICLE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string STAGE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string KIND { get; set; }

        public int PASS { get; set; }

        public int FAIL { get; set; }

        [Required]
        [StringLength(10)]
        public string LABNO { get; set; }

        [Required]
        [StringLength(100)]
        public string FILENAME { get; set; }
    }
}