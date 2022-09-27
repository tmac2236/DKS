using System;

namespace DKS_API.DTOs
{
    public class DtrF206BomDto
    {
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Season { get; set; }
        public string Stage { get; set; }
        public string Article { get; set; }

        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string SupplierMatId { get; set; }
        //public string MateriaNo { get; set; }
        public string MaterialName { get; set; }

        public string Supplier { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string Uom { get; set; }
        public string IsSize { get; set; }

        public string MaterialSize { get; set; }
        public Decimal Consumption { get; set; }
    }
}