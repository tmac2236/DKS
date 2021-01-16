using System.ComponentModel.DataAnnotations;

namespace DKS.API.Models.DKSSys
{
    public class Jang1HR
    {
        [Key]
        public System.Guid Fid { get; set; } //工作編號

        [StringLength(50)]
        public string ProdGroup { get; set; } //工段
        [StringLength(50)]
        public string LineTypeNr { get; set; } //線別代號
        [StringLength(50)]
        public string LineTypeName { get; set; } //線別名稱  
        [StringLength(50)]
        public string Tung { get; set; } //棟別
        [StringLength(50)]
        public string ProdDept { get; set; } //部門別


    }
}