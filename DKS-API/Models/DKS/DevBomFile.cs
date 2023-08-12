using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevBomFile
    {
        [Key]
        public string FACTORY { get; set; }

        public string DEVTEAMID { get; set; }

        public string SEASON { get; set; }

        public string MODELNO { get; set; }

        public string MODELNAME { get; set; }

        [Key]
        public string ARTICLE { get; set; }

        public string STAGE { get; set; }

        [Key]
        public short VER { get; set; }

        public string FILENAME { get; set; }

        public string REMARK { get; set; }

        public string APPLY { get; set; }

        public DateTime? UPDAY { get; set; }

        public string UPUSR { get; set; }
    }
}