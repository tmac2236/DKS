using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class PrdEntryAccessDto
    {
        public string SampleNo { get; set; }
        public string BarcodeNo { get; set; }
        public string Epc { get; set; }
        public string SecurityCode { get; set; }
        public string Article { get; set; }

        public string ModelNo { get; set; }
        public string ModleName { get; set; }
        public string Reason { get; set; }
        public string Gate { get; set; }       
        public string Keeper { get; set; }   

        public string AlarmStatus { get; set; }       
        public string RecordTime { get; set; }   
    }
}