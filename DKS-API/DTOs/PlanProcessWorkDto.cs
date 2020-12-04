namespace DFPS.API.DTOs
{
    public class PlanProcessWorkDto
    {
        public string Maker { get; set; }      //組別
        public string SampleNr { get; set; }   //型體
        public string SampleDesc { get; set; }
        public int? OrdinalNum { get; set; }
        public string ProcessNum { get; set; }    //工序
        public string Description { get; set; }    //工序說明
        public string WorkerNum { get; set; }    //工作者編號
        public string WorkerName { get; set; }   //工作者名稱

    }
}