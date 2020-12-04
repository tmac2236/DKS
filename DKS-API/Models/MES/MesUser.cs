using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFPS.API.Models.MES
{
    public class MesUser
    {
        [Key]
        [Column(Order = 0)]
        public string Factory_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string User_Type { get; set; }
        public string DAQ_Type { get; set; }
        public string PDC_ID { get; set; }
        public string Line_ID { get; set; }
        public string Dept_ID { get; set; }

        public string Email { get; set; }
        public DateTime? Update_Time { get; set; }
        public string Updated_By { get; set; }
    }
}