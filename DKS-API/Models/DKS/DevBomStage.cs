using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevBomStage
    {
        [Key]
        public string FACTORY { get; set; }

        [Key]
        public string STAGE { get; set; }


        public short SORT { get; set; }

        public DateTime? UPDAY { get; set; }

        public string? UPUSR { get; set; }

        
    }
}