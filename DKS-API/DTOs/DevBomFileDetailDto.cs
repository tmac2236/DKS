using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevBomFileDetailDto
    {

    public string FactoryId { get; set; }
    public string DevTeamId { get; set; }
    public string TeamName { get; set; }
    public string Season { get; set; }
    public string ModelNo { get; set; }

    public string ModelName { get; set; }
    public string Article { get; set; }
    public string Stage { get; set; }
    public string ActDate { get; set; }
    public string FileName { get; set; }

    public short Ver { get; set; }
    public short Sort { get; set; }
    public string Remark { get; set; }
    public string Apply { get; set; }
    //public long Id { get; set; }
    public string RemarkButton { get; set; }
    public string DownloadButton { get; set; }
    public string ApplyButton { get; set; }
    public string UploadButton { get; set; }

    public string CwaDeadLine { get; set; }
    public string PdmApply { get; set; }
    public string EcrNo { get; set; }

    }
}