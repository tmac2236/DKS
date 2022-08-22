using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class TempSamplQtb
    {


        [Required]
        public string SAMPLENO { get; set; }

        [Required]
        public string PASSIDNAME { get; set; }

    }
}