using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DKS.API.Models.DKSSys
{
    public class Jang1Base
    {
        [Key]
        [StringLength(20)]
        public string Customernr { get; set; } //工作編號
        [StringLength(40)]
        public string? Customername { get; set; }//工作名稱
        public int? FTypeFlag { get; set; }      //員工:1
        [StringLength(50)]
        public string? PordDept { get; set; }    //工段
        public int? OnDuty { get; set; }        //出勤
        [StringLength(50)]
        public string? Attendance { get; set; } //考勤

    }
}