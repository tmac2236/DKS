using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class F418_F420Dto
    {
        [StringLength(15)]
        public string PROORDNO { get; set; }
        [Column(TypeName = "decimal(7,1)")]
        public decimal NEEDQTY { get; set; }
    }
}