using System;
using System.ComponentModel.DataAnnotations;
namespace DFPS.API.Models.DKSSys
{
    public class SysUser
    {
        [Key]
        public long USERID { get; set; }
        public string LOGIN { get; set; }
        public System.Guid? ENTERPRISEID { get; set; }
        public string USERNAME { get; set; }
        public string USERNICKNAME { get; set; }

        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public System.Guid? COUNTRYID { get; set; }
        public string? MOBILEPHONE { get; set; }
        public string? EXTENSION { get; set; }

        public bool SITEMANAGER { get; set; }
        public DateTime MKDATE { get; set; }
        public long MKUSERID { get; set; }
        public DateTime? LUDATE { get; set; }
        public long? LUUSERID { get; set; }

        public byte STATUS { get; set; }
        public DateTime? LOGINDATE { get; set; }
        public DateTime? LOGOUTDATE { get; set; }
        public DateTime? LASTPWCHGDATE { get; set; }
        public byte PWERRORCOUNT { get; set; }

        public DateTime? EFFECTIVEDATE { get; set; }
        public long? EFFECTIVEUSERID { get; set; }
        public DateTime? INVALIDDATE { get; set; }
        public long? INVALIDUSERID { get; set; }
        public string? BACKUPEMAIL { get; set; }
    }
}