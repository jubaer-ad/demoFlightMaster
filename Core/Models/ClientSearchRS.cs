namespace Core.Models
{
    public class ClientSearchRS : SupplierSearchRS
    {
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
