using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class ModelDab
    {
        [StringLength(15)]
        public string MODELNO { get; set; }

        [StringLength(20)]
        public string SHOESIZE { get; set; }
 
    }
}