
namespace Core.Models
{
    public class SupplierSearchRQ : ClientSearchRQ
    {
        public ApiCredential ApiCredential { get; set; }
        public decimal ConvertionRate { get; set; }
        public string TrnxId { get; set; }
    }

    public class ApiCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string? ApiKey { get; set; }
    }
}
