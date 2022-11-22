using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class PrdRfidAlertDto
    {
        public string Gate { get; set; }
        public DateTime Time { get; set; }
        public string Seq { get; set; }
        public string Epc { get; set; }
        public string Reason { get; set; }
        public string Updater { get; set; }
        public string UpdateTime { get; set; }
        
    }
}