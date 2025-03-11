
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class SupplierSearchRQ : ClientSearchRQ
    {
        [Required]
        public required ApiCredential ApiCredential { get; set; }
        public decimal ConvertionRate { get; set; } = 1;
        [Required]
        public required string TrnxId { get; set; }
    }

    public class ApiCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string? ApiKey { get; set; }
    }
}
