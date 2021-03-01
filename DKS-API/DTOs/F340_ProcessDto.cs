using System;
using System.ComponentModel.DataAnnotations;

namespace DKS_API.DTOs
{
    public class F340_ProcessDto
    {

        public string BuyPlanSeason { get; set; }
        public string VersionNo { get; set; }
        public string DevSeason { get; set; }
        public string DevTeam { get; set; }
        public string Article { get; set; }

        public string CwaDeadline { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string OrderStag { get; set; }
        public string SampleNo { get; set; }

        public string DevStatus { get; set; }
        public string DropDate { get; set; }
        public string HpFlag { get; set; }
        public string HpSampleNo { get; set; }
        public string F340SampleNo { get; set; }

        public string ReleaseType { get; set; }
        public string CreateDate { get; set; }
        public string PdmDate { get; set; }
        public string DevUpDate { get; set; }
        public string DevBtmDate { get; set; }

        public string TTUpDate { get; set; }
        public string TTBtmDate { get; set; }
        public string ReleaseDate { get; set; }
        public string TTRejectReason { get; set; }
        public string TTRejectDate { get; set; }

        public string TTRejectCount { get; set; }

    }
}