using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Pest.Models
{
    public class DSR_ChemicalStockIn
    {
        public int ChemicalStockId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Date { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ChemicalName { get; set; }
        public int ChemicalId { get; set; }
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string BatchNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExpiryDate { get; set; }

        public int Uid { get; set; }
        public int BranchId { get; set; }
    }
}