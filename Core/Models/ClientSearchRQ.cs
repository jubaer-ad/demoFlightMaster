using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ClientSearchRQ
    {
        public List<Route> Routes { get; set; }

        [Range(0, int.MaxValue)]
        public int Adult { get; set; }

        [Range(0, int.MaxValue)]
        public int Child { get; set; }

        [Range(0, int.MaxValue)]
        public int Infant { get; set; }
    }
    public class Route
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
    }

}
