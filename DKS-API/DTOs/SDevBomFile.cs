using System;
using DKS_API.Helpers;

namespace DKS_API.DTOs
{
    public class SDevBomFile : PaginationParams
    {
        public string FactoryId { get; set; }
        public string Season { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Article { get; set; }
        public string Team { get; set; }
        public string UserTeam { get; set; }
        

    }
}