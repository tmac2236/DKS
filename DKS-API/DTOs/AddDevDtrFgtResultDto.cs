using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class AddDevDtrFgtResultDto
    {
        public string Article { get; set; }
        public string Stage { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public string ModelNo { get; set; }

        public string ModelName { get; set; }
        public string LabNo { get; set; }
        public string Result { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        
        public string FileName { get; set; }
        public string Remark { get; set; }
        public DateTime Upday { get; set; }
        public string Upusr { get; set; }
    }
}