using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class ArticlePicture
    {
        [Key]
        [StringLength(12)]
        public string FKARTICID { get; set; }

        public byte[] PICTURE { get; set; }

        [Required]
        [StringLength(255)]
        public string FILE_NAME { get; set; }
    }
}