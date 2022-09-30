using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class SrfDifferenceDto
    {

        public string ModelNo { get; set; }
        public string PartNumber { get; set; }
        public string SrfId1 { get; set; }  
        public string PartName1 { get; set; }
        public string SrfId2 { get; set; }                   
        public string PartName2 { get; set; }
    }
}