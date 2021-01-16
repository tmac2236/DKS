using System;
namespace DKS_API.DTOs
{
    public class ChangeWorkerDto
    {
        public DateTime? StartDate { get; set; }
        public string Lean { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string ProdNum { get; set; }
        public string Description { get; set; }
        public string ProcessMemo { get; set; }
        public string Team { get; set; }
        public string WorkerNum { get; set; }
        public string WorkerName { get; set; }
        public string WorkLevel { get; set; }
        public string OldTeam { get; set; }
        public string OldWorkerNum { get; set; }
        public string OldWorkerName { get; set; }
    }
}