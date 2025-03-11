namespace Core.Models
{
    public class SupplierSearchRS
    {
        public string TrnxId { get; set; }
        public string ItemCodeRef { get; set; }
        public PriceComponent FlightPrice { get; set; }
        public PassengerFares PassengerFares { get; set; }

        public List<Direction> Directions { get; set; }
        public string FlightNumber { get; set; }
    }
    public class PassengerFares
    {
        public PaxPriceComponent? Adult { get; set; }
        public PaxPriceComponent? Child { get; set; }
        public PaxPriceComponent? Infant { get; set; }
    }

    public class PriceComponent
    {
        public decimal BasePrice { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    public class PaxPriceComponent : PriceComponent
    {
        public int Count { get; set; }
    }
    public class Direction
    {
        public string From { get; set; }
        public string To { get; set; }
        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }
        public string PlatingCarrierCode { get; set; }
        public string PlatingCarrierName { get; set; }
        public int Stops { get; set; }
        public string TravelTime { get; set; }
    }
}
