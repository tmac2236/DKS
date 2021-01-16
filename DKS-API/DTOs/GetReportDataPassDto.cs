using System;

namespace DKS_API.DTOs
{

    public class GetReportDataPassDto
    {
        public DateTime? CDate { get; set; }
        public string Maker { get; set; }
        public string SampleDesc { get; set; }
        public string ModelNr { get; set; }
        public string ProcessNum { get; set; }
        public string ProcessMemo { get; set; }
        public string Description { get; set; }
        public string PordDept { get; set; }
        public string WorkerNum { get; set; }
        public string WorkerName { get; set; }
        public int? WorkLevel { get; set; }
        public int? WorkLevel2 { get; set; }
        public DateTime? EditDate { get; set; }

    }
}