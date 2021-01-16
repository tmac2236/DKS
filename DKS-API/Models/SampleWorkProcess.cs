using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DKS.API.Models.DKSSys
{
    public class SampleWorkProcess
    {
        [Key]
        public System.Guid Fid { get; set; }
        public System.Guid? Hid { get; set; }      //組別
        [StringLength(20)]
        public string? ProcessNum { get; set; }    //工序
        [StringLength(512)]
        public string? Description { get; set; }    //工序說明
        public int? OrdinalNum { get; set; }


    }
}