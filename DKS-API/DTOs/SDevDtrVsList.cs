using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevDtrVsList : PaginationParams
    {
        public string Season { get; set; }
        public string Article { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string DeveloperId { get; set; }
        public string DevTeamId { get; set; }
    }
}