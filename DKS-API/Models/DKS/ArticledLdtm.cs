using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class ArticledLdtm
    {
        public int SHOES_LEAD_TIME { get; set; }
        public byte[] ATTACHED_DATA { get; set; }
        [StringLength(255)]
        public string ATTACHED_DATA_NAME { get; set; }
        [StringLength(20)]
        public string CONFIRM_STATUS { get; set; }
        [StringLength(5)]
        public string CHANGPID { get; set; }
        public DateTime? CHANGDATE { get; set; }
        [StringLength(200)]
        public string NOTE { get; set; }
        [Key]
        [StringLength(12)]
        public string PKARTBID { get; set; }
    }
}