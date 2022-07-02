using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class F505Dto
    {

        public string MtDocNo { get; set; }
        public string WareCode { get; set; }
        public string Location { get; set; }
        public string MtTypeId { get; set; }
        public string Season { get; set; }

        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Last { get; set; }
        public string SheSize { get; set; }
        public string Memo { get; set; }

        public decimal StockQty { get; set; }

    }
}