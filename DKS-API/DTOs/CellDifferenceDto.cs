using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class CellDifferenceDto
    {
        public string CellName { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
    }
}