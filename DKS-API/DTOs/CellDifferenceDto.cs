using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aspose.Cells;

namespace DKS.API.Models.DKS
{
    public class CellDifferenceDto
    {
        public string CellName { get; set; }
        public Cell NewValue { get; set; }
        public Cell OldValue { get; set; }
    }
}