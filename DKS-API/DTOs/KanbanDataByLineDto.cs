
using System;

namespace DKS.API.Models.DKS
{
    public class KanbanDataByLineDto
    {

        public string LINE_ID { get; set; }
        public DateTime PRODDATE { get; set; }
        public string SAMPLENO { get; set; }
        public string PRODUCESTATUS { get; set; }
        public decimal PRODUCEQTY { get; set; }

        public string RESPONSIBILITY { get; set; }
        public decimal? defect_qty { get; set; }
        public string RFT { get; set; }
        public byte DL { get; set; }
        public string MODELNAME { get; set; }

        public string MODELNO { get; set; }
        public decimal PLANQTY { get; set; }
        public decimal TotalDEFECT_QTY { get; set; }
        public decimal TotalPRODUCEQTY { get; set; }
        public string TRFT { get; set; }


    }
}