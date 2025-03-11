using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class SupplierBookRS
    {

        [Required]
        public required List<PassengerData> Passengers { get; set; }
        [Required]
        public required string PNR { get; set; }
        public required string TrnxId { get; set; }
        public DateTime? LastTicketingTimeLimit { get; set; }
        public required string Status { get; set; }
        public PriceComponent FlightPrice { get; set; }
        public PassengerFares PassengerFares { get; set; }

        public List<Direction> Directions { get; set; }
        public string FlightNumber { get; set; }
    }
}