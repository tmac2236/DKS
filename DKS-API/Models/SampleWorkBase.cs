using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DFPS.API.Models
{
    public class SampleWorkBase
    {
        [Key]
        public System.Guid Fid { get; set; }
        [StringLength(20)]
        public string? Maker { get; set; }      //組別
        [StringLength(40)]
        public string? SampleNr { get; set; }   //型體
        [StringLength(512)]
        public string? SampleDesc { get; set; }     //型體名稱
        [StringLength(12)]
        public string? ColorNr { get; set; }    //顏色

    }
}