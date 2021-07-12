using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevDtrFgt
    {

        [Key]
        public string ARTICLE { get; set; }
        [Key]
        public string STAGE { get; set; }
        [Key]
        public string KIND { get; set; }
        [Key]
        public int VERN { get; set; }
        public string LABNO { get; set; }

        public string PASS { get; set; }
        public string FAIL { get; set; }
        public string FILENAME { get; set; }
        public DateTime UPDAY { get; set; }
        public string UPUSR { get; set; }
    }
}