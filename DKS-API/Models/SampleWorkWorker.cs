using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DKS.API.Models.DKSSys
{
    public class SampleWorkWorker
    {
        [Key]
        public System.Guid Fid { get; set; }
        public System.Guid? Hid { get; set; }
        [StringLength(20)]
        public System.Guid? Bid { get; set; }
        [StringLength(20)]
        public string? WorkerNum { get; set; }    //工作者編號
        [StringLength(40)]
        public string? WorkerName { get; set; }   //工作者名稱


    }
}