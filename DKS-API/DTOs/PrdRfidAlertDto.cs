using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class PrdRfidAlertDto
    {
        public string Gate { get; set; }
        public DateTime Time { get; set; }
 
    }
}