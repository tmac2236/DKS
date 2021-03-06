namespace DKS_API.DTOs
{
    public class StockDetailByMaterialNo
    {

        public string Kind { get; set; }
        public string MateriaNo { get; set; }
        public string OrderStage { get; set; }
        public string Article { get; set; }
        public string ModelNo { get; set; }

        public string StockNo { get; set; }
        public string Location { get; set; }
        public string MaterialQty { get; set; }
        public string TestResult { get; set; }
        public string InBoxDate { get; set; }

    }
}