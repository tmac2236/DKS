using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFPS.API.DTOs
{
    public class NoOperationDto
    {
        public DateTime? StartDate { get; set; }
        public string Lean { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Status { get; set; }
        public int? IOrder { get; set; }
        public int? OnDuty { get; set; }
    }
}