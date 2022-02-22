using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DtrFgtEtdDto
    {
        public string FactoryId { get; set; }
        public string Article { get; set; }
        public string Stage { get; set; }
        public string Test { get; set; }
        public DateTime QcReceive { get; set; }

        public DateTime? QcEtd { get; set; }
        public string Remark { get; set; }
        public string? EtdUser { get; set; }
        public DateTime? EtdDate { get; set; }
        public string? LabNo { get; set; }
        
        public string? FgtUser { get; set; }
        public DateTime? FgtDate { get; set; }
    }
}