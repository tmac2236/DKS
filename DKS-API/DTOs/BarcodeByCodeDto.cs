using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class BarcodeByCodeDto
    {
        public DateTime DataDate { get; set; }
        public string BarcodeNo { get; set; }
        public string Article { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Code { get; set; }

        public string TransferTo { get; set; }
        public string TransferBy { get; set; }
        public string DataType { get; set; }
         public string ProcessNo { get; set; }
        public string ReformNo { get; set; }
    }
}