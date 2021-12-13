using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevPlmPart
    {
        [Key]
        public string PARTNO { get; set; }
        public string PARTNAMEEN { get; set; }
        public string PARTNAMECN { get; set; }
        public string PARTNAMEVN { get; set; }
        public string LOCATION { get; set; }
        public string RENAME { get; set; }
        public string PARTGROUP { get; set; }
        public string INSERTUSER { get; set; }
        public DateTime? INSERTDATE { get; set; }
        public string CHANGEUSER { get; set; }
        public DateTime? CHANGEDATE { get; set; }
    }
}