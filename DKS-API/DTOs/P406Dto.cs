using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class P406Dto
    {
        public string? FmcaqNo { get; set; }
        public DateTime? FmcaqDate { get; set; }
        public string? MateType { get; set; }
        public string? EtpId { get; set; }
        public string? FactoryId { get; set; }

        public string? TestResult { get; set; }
        public DateTime AcptDate { get; set; }
        public string? Color { get; set; }
        public string? MateCate { get; set; }
        public string? ModelNameMo { get; set; }
        
        public string? Reject { get; set; }
        public string? Descript { get; set; }
        public string? Location { get; set; }
        public string? AcceptNo { get; set; }
        public string? OrderNum { get; set; }

        public string? ModelNo255 { get; set; }
        public string? Articl255 { get; set; }
        public string? DevTeamName { get; set; }
        public string? Odstage20 { get; set; }
        public string? MaterNo { get; set; }

        public string? MaterName { get; set; }
        public string? MsuppCode { get; set; }
        public string? MsuppName { get; set; }
        public string? Sample { get; set; }
        public string? Season { get; set; }

        public string? Stage { get; set; }

    }
}