using System;
using System.ComponentModel.DataAnnotations;

namespace DKS.API.Models.DKS
{
    public class DtrLoginHistory
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SystemName { get; set; }

        [StringLength(50)]
        public string Account { get; set; }

        [StringLength(50)]
        public string PcName { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public DateTime? LoginTime { get; set; }
    }
}