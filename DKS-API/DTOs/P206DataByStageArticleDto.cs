namespace DKS_API.DTOs
{
    public class P206DataByStageArticleDto
    {

        public string Title { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }

        public string Supplier { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string Uom { get; set; }
        public string Size { get; set; }

        public string MaterialSize { get; set; }
        public string Consumption { get; set; }
        public string Purchasing { get; set; }
        public string Grading { get; set; }
        public string MaterialStatus { get; set; }

        public string SampleLeadTimeEtd { get; set; }
        public string ProductionLeadTimeEtd { get; set; }
        public string PurchaseLocation { get; set; }

    }
}