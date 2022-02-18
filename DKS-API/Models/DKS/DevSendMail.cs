using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevSendMail
    {
        [Key]
        public string WORKPNO { get; set; }
        public string? NAME { get; set; }
        public Decimal STATUS { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime CREATE_TIME { get; set; }

        public string? UPDATE_BY { get; set; }
        public DateTime? UPDATE_TIME { get; set; }
        [Key]
        public string DEPTID { get; set; }
        public string? EMAIL { get; set; }
        [Key]
        public string EMAIL_TYPE { get; set; }
        [Key]
        public string DEVTEAM { get; set; }
    }
}