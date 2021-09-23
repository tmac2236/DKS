using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevDtrVsFile
    {
        [Key]
        public string FACTORYID { get; set; }
        [Key]
        [StringLength(6)]
        public string ARTICLE { get; set; }

        [Key]
        [StringLength(4)]
        public string SEASON { get; set; }

        [Key]
        [StringLength(5)]
        public string ID { get; set; }

        [StringLength(500)]
        public string REMARK { get; set; }

        [StringLength(40)]
        public string FILENAME { get; set; }

        public DateTime UPDAY { get; set; }
        
        [StringLength(20)]
        public string UPUSR { get; set; }

    }
}