using System.ComponentModel.DataAnnotations;

namespace DKS_API.DTOs
{
    public class F418_F420Dto
    {
        [StringLength(15)]
        public string PROORDNO { get; set; }
        public decimal NEEDQTY { get; set; }
    }
}