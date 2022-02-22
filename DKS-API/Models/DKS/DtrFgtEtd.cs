using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DtrFgtEtd
    {
        [Key]
        public string FACTORYID { get; set; }
        [Key]
        public string ARTICLE { get; set; }
        [Key]
        public string STAGE { get; set; }
        [Key]
        public string TEST { get; set; }
        [Key]
        public DateTime QC_RECEIVE { get; set; }
        public DateTime? QC_ETD { get; set; }
        public string? REMARK { get; set; }
        public string? UPUSR { get; set; }
        public DateTime? UPDAY { get; set; }
    }
}