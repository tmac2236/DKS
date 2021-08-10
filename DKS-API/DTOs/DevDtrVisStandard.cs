using Microsoft.AspNetCore.Http;

namespace DKS_API.DTOs
{
    public class DevDtrVisStandard 
    {
        public string Article { get; set; }
        public string Season { get; set; }
        public string Id { get; set; }
        public string LoginUser { get; set; }
        public IFormFile File { get; set; }
    }
}