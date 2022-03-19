using System.ComponentModel.DataAnnotations.Schema;

namespace DKS_API.DTOs
{
    public class P202Dto
    {

        public string MATERNAME { get; set; }
        public string COLOR { get; set; }
        public string TYPE { get; set; }
        public string CWADATE { get; set; }
        public string DEVSTATUS { get; set; }

        public string LABNO { get; set; }
        public string TESTRESLT { get; set; }
        public string SUPSNAME { get; set; }
        public string EVALUDATE { get; set; }
        public string EXPIREDATE { get; set; }

        public string BRANDCATE { get; set; }
        public string STAGE { get; set; }
        public string ARTICLE { get; set; }
        public string SEASON { get; set; }
        public string MODELNAME { get; set; }

        public string MODELNO { get; set; }
        public string DEVTEAMNAME { get; set; }
        public string PARTNO { get; set; }
        public string PARTNAME { get; set; }
        public string MATERIANO { get; set; }

        public string SSBMATPID { get; set; }
        public string LEADTIME { get; set; }
        public string PRODLDTM { get; set; }
        public string EUTECTIC_POINT { get; set; }
        public string MOQ { get; set; }

        public string PRODUCMOQ { get; set; }
        public string REMARK { get; set; }
        public string RPODCOUNNAME { get; set; }
        public string TEST_RESULT { get; set; }
        public string EXPIRE_DATE { get; set; }

    }
}