using System.ComponentModel.DataAnnotations;

namespace DKS_API.DTOs
{
    public class F340_ProcessDto
    {

        public string ModelNo { get; set; }
        public string Article { get; set; }
        public string SampleNo { get; set; }
        public string VerNo { get; set; }
        public string ReleaseType { get; set; }

        public string CwaDate { get; set; }
        public string CreateDate { get; set; }
        public string PdmDate { get; set; }
        public string DevUpperDate { get; set; }
        public string DevBottomDate { get; set; }

        public string TTDate { get; set; }
        public string ReleaseDate { get; set; }
        public string TTRejectReason { get; set; }
        public string TTRejectDate { get; set; }

    }
}