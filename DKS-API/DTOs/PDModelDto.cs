using System;
namespace DFPS.API.DTOs
{
    public class PDModelDto
    {
        public string PDSerialNum { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Lean { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
    }
}