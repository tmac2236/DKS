namespace DKS_API.DTOs
{
    //F340 with PartNo、 treamentcode Chinese and English Name
    public class F340PartNoTreatmemtDto
    {

        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentZh { get; set; }
        public string TreatmentEn { get; set; }

    }
}