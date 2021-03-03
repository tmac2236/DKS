using System;
using System.ComponentModel.DataAnnotations;

namespace DKS_API.DTOs
{
    public class F428SampleNoDetail
    {

        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNo { get; set; }
        public string SsbMatPid { get; set; }

        public string SupName { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string Uom { get; set; }
        public string Cons { get; set; }

        public string Total { get; set; }
        public string Status { get; set; }
        public string CheckStockNo { get; set; }
        public string CheckUser { get; set; }
        public string CheckTime { get; set; }

    }
}