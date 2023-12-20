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
        [Key]
        public string STAGE { get; set; }

        public short VER { get; set; }

        [Key]
        public short SORT { get; set; }

        public string FILENAME { get; set; }

        public string REMARK { get; set; }

        public string APPLY { get; set; }

        public string ECRNO { get; set; }

        public DateTime? UPDAY { get; set; }

        public string UPUSR { get; set; }
        public string PDM_APPLY { get; set; }
        public string PDM_UPUSR { get; set; }
        public DateTime? PDM_UPDAY { get; set; }
        public string ARTICLE_LIST { get; set; }
        
        
    }
}