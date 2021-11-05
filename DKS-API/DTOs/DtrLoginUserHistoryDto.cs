using System;

namespace DKS_API.DTOs
{
    public class DtrLoginUserHistoryDto
    {
        public string SystemName { get; set; }
        public string FactoryId { get; set; }
        public string Account { get; set; }
        public string WorkNo { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public DateTime? LoginTime { get; set; }
    }
}