
using System;

namespace DKS.API.Models.DKS
{
    public class KanbanTQCDto
    {

        public DateTime PLANDATE { get; set; }
        public string LINE_ID { get; set; }
        public string DEFECTCODE { get; set; }
        public string DEFECTNAME_EN { get; set; }
        public int DEFECT_TIMES { get; set; }

        public DateTime CHANGDATE { get; set; }
        public string RESPONSIBILITY { get; set; }
        public byte DL { get; set; }
        public long TOP_NUM { get; set; }
        public string PICTURE { get; set; }


    }
}