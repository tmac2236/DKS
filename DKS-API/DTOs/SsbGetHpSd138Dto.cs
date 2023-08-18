using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class SsbGetHpSd138Dto
    {
        public string manuf { get; set; }
        public string ecrno { get; set; }
        public string style { get; set; }
        public short seq { get; set; }
        public string article { get; set; }

    }
}