using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class SrfChangeDto
    {

        public string SrfIdN { get; set; }
        public string ModelNoN { get; set; }
        public DateTime InsertDateN { get; set; }
        public string SrfIdO { get; set; }  
        public string ModelNoO { get; set; }         
        public DateTime InsertDateO { get; set; }
        public string Article { get; set; }
    }
}