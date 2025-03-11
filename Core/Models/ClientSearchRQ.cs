namespace Core.Models
{
    public class ClientSearchRQ
    {
        public List<Route> Routes { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }

    }
    public class Route
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
    }

}
