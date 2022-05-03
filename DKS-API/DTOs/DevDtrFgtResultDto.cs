using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevDtrFgtResultDto
    {

        public string Article { get; set; }
        public string Stage { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public string ModelNo { get; set; }

        public string ModelName { get; set; }
        public string LabNo { get; set; }
        public string Result { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }

        public string FileName { get; set; }
        public string Remark { get; set; }
        public string Upday { get; set; }
        public string Upusr { get; set; }
        public string TreatmentCode { get; set; }
        
        public string TreatmentZh { get; set; }
        public string TreatmentEn { get; set; }
        public string FirstUpday { get; set; }     //2022 04 29 add
        
        public string Vern { get; set; }  //配合和新增畫面一樣物件(Buy Plan)
        public string DevSeason { get; set; }  //配合和新增畫面一樣物件(Buy Plan、Dev)
        public string CwaDate { get; set; }  //配合和新增畫面一樣物件(Buy Plan)
        public string Factory { get; set; }  //配合和新增畫面一樣物件(Buy Plan)
        public string BuyPlanSeason { get; set; }  //配合和新增畫面一樣物件(Buy Plan)

    }
}