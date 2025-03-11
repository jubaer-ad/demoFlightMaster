using Core.Helper;
using Core.Models;
using Newtonsoft.Json;

namespace SupplierOne.Services
{
    public class FlightService
    {
        //
        // Summary:
        //      This method handle actual supplier api call and the supplier will handle the authentication and authorization based on the provided ApiCredentials
        //
        public async Task<List<SupplierSearchRS>> HandleSearchAsync(SupplierSearchRQ request)
        {
            FileHelper.JsonFileSaveAsync($"SupplierOne-{request.TrnxId}-RQ", JsonConvert.SerializeObject(request), "SupplierOne", "Search");

            var rs = await Task.Run(() =>
            {
                List<SupplierSearchRS>? rs = [];

                // Mimicking the response from actual supplier, adding randomness in response
                Random rnd = new();
                int flightCount = rnd.Next(10, 33);

                for (int i = 0; i < flightCount; i++)
                {
                    SupplierSearchRS flight = new()
                    {
                        FlightNumber = "FL" + rnd.Next(100, 999),
                        FlightPrice = new PriceComponent
                        {
                            BasePrice = rnd.Next(100, 1000),
                            Tax = rnd.Next(10, 100),
                            Total = rnd.Next(100, 1000)
                        },
                        ItemCodeRef = "FL" + rnd.Next(100, 999),
                        TrnxId = request.TrnxId,
                        PassengerFares = new PassengerFares
                        {
                            Adult = request.Adult > 0 ? new()
                            {
                                BasePrice = rnd.Next(100, 1000),
                                Tax = rnd.Next(10, 100),
                                Total = rnd.Next(100, 1000),
                                Count = request.Adult
                            } : default,
                            Child = request.Child > 0 ? new()
                            {
                                BasePrice = rnd.Next(100, 1000),
                                Tax = rnd.Next(10, 100),
                                Total = rnd.Next(100, 1000),
                                Count = request.Child
                            } : default,
                            Infant = request.Infant > 0 ? new()
                            {
                                BasePrice = rnd.Next(100, 1000),
                                Tax = rnd.Next(10, 100),
                                Total = rnd.Next(100, maxValue: 1000),
                                Count = request.Infant
                            } : default
                        },
                        Directions = []
                    };

                    foreach (var item in request.Routes)
                    {
                        flight.Directions.Add(new Direction
                        {
                            From = item.Origin,
                            To = item.Destination,
                            FromAirport = "Airport" + rnd.Next(100, 999),
                            ToAirport = "Airport" + rnd.Next(100, 999),
                            PlatingCarrierCode = "PC" + rnd.Next(100, 999),
                            PlatingCarrierName = "Carrier" + rnd.Next(100, 999),
                            Stops = rnd.Next(0, 2),
                            TravelTime = rnd.Next(1, 10) + "h " + rnd.Next(1, 59) + "m"
                        });
                    }
                    rs.Add(flight);
                }
                return rs;
            });


            FileHelper.JsonFileSaveAsync($"SupplierOne-{request.TrnxId}-RS", JsonConvert.SerializeObject(rs), "SupplierOne", "Search");
            return rs;
        }

        public async Task<SupplierBookRS?> HandleBookAsync(SupplierBookRQ request)
        {

            try
            {
                FileHelper.JsonFileSaveAsync(
                    $"SupplierOne-{request.TrnxId}-RQ",
                    JsonConvert.SerializeObject(request),
                    "SupplierOne",
                    "Book");

                var searchRSStr = await FileHelper.JsonFileRetriveAsync(
                    $"SupplierOne-{request.TrnxId}-RS",
                    "SupplierOne",
                    "Search");

                if (string.IsNullOrEmpty(searchRSStr))
                    return null;

                var searchRS = JsonConvert.DeserializeObject<List<SupplierSearchRS>>(searchRSStr);

                var selectedSearchRS = searchRS?.FirstOrDefault(x => x.ItemCodeRef == request.ItemCodeRef)
                    ?? throw new Exception("Search response not found with the item key provided"); // Can be replaced with custom logic other than throwing exception as throwing exception is not a good practice

                

                var rs = await Task.Run(() =>
                {
                    SupplierBookRS supplierBookRS = new()
                    {
                        Passengers = [],
                        PNR = Helper.Misc.GeneratePNR(),
                        Status = "Booked",
                        TrnxId = request.TrnxId,
                        LastTicketingTimeLimit = DateTime.Now.AddHours(24),
                        Directions = selectedSearchRS.Directions,
                        FlightNumber = selectedSearchRS.FlightNumber,
                        FlightPrice = selectedSearchRS.FlightPrice,
                        PassengerFares = selectedSearchRS.PassengerFares
                    };



                    return supplierBookRS;
                });

                FileHelper.JsonFileSaveAsync(
                    $"SupplierOne-{request.TrnxId}-RS",
                    JsonConvert.SerializeObject(rs),
                    "SupplierOne",
                    "Book");
                return rs;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
