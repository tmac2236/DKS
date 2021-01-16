using Microsoft.AspNetCore.Http;

namespace DKS.API.Models.DKS
{
    public class ArticlePic
    {
        public IFormFile File { get; set; }
        public string Article { get; set; }
        public string No { get; set; }
        public string User { get; set; }
    }
}