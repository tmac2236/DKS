using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevGateLogDataLog
    {
        [Key]
        public int SEQ { get; set; }

        public string REASON { get; set; }

        public string UPDATER { get; set; }

        public string? FLAG { get; set; }

        public string UPDATETIME { get; set; }
        
    }
}