using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class SupplierBookRQ : ClientBookRQ
    {
        [Required]
        public required ApiCredential ApiCredential { get; set; }
        public decimal ConvertionRate { get; set; } = 1;
    }
}
