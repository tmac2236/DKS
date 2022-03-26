using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class SampleTrackReportDto
    {

        public string SampleNo { get; set; }
        public string Article { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Team { get; set; }

        public string Stage { get; set; }
        public string Test { get; set; }
        public string BarCodeNo { get; set; }
        public DateTime DevTransDate { get; set; }
        public string DevTransSn { get; set; }

        public string DevKeeper { get; set; }
        public string DevTransferer { get; set; }
        public int OverDays { get; set; }
        public string DeptName { get; set; }
        public string TransferName { get; set; }

        

    }
}