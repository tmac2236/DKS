using Microsoft.AspNetCore.Http;

namespace DKS_API.DTOs
{
    public class UploadDevBomFileDto 
    {
        public string FactoryId { get; set; }
        public string Team { get; set; }
        public string Season { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }

        public string Article { get; set; }
        public string Stage { get; set; }
        public string Ver { get; set; }
        public string Remark { get; set; }
        public string UpdateUser { get; set; }
        public IFormFile File { get; set; }

        public string Sort { get; set; }
        public string Ecrno { get; set; }
        public string PdmApply { get; set; }
        public string PdmUpusr { get; set; }
        public string PdmUpday { get; set; }
        
        public string ArticleList { get; set; }
    }
}