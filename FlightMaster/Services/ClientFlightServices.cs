using Core.Models;
using Core.Helper;
using Newtonsoft.Json;

namespace FlightMaster.Services
{
    public class ClientFlightServices(IConfiguration configuration)
    {
        public async Task<List<ClientSearchRS?>> HandleSearchAsync(ClientSearchRQ request)
        {
            try
            {
                var trnxId = Guid.NewGuid().ToString("D");

                FileHelper.JsonFileSaveAsync($"Master-{trnxId}-RQ", JsonConvert.SerializeObject(request), "SupplierOne", "Search");

                Random rnd = new();
                // Call the supplier service
                var supplierSearchRQ = new SupplierSearchRQ
                {
                    ApiCredential = new ApiCredential
                    {
                        UserName = "user",
                        Password = "password",
                        Url = "http://supplierone.com",
                        ApiKey = "key"
                    },
                    ConvertionRate = 1.0m,
                    Adult = request.Adult,
                    Child = request.Child,
                    Infant = request.Infant,
                    Routes = request.Routes,
                    TrnxId = trnxId
                };
                var baseUrl = configuration.GetSection("SuppliersURL:SupplierOne").Value ?? string.Empty;

                var supplierSearchRSStr = await ReqRspHelper<SupplierSearchRQ>.PostRequest(
                    supplierSearchRQ,
                    baseUrl + "/api/flight/search"
                    );

                if (string.IsNullOrEmpty(supplierSearchRSStr))
                    return null;

                var supplierSearchRS = JsonConvert.DeserializeObject<List<SupplierSearchRS>>(supplierSearchRSStr);

                var rs = new List<ClientSearchRS?>();
                // Convert the supplier response to client response
                foreach (var item in supplierSearchRS)
                {
                    var discountPercentage = rnd.Next(2, 6) / 100m;
                    var clientSearchRS = new ClientSearchRS
                    {
                        Directions = item.Directions,
                        FlightNumber = item.FlightNumber,
                        FlightPrice = item.FlightPrice,
                        ItemCodeRef = item.ItemCodeRef,
                        PassengerFares = item.PassengerFares,
                        TrnxId = item.TrnxId,
                        Discount = item.FlightPrice.Total * discountPercentage,
                        DiscountPercentage = discountPercentage
                    };
                    rs.Add(clientSearchRS); 
                }


                FileHelper.JsonFileSaveAsync($"Master-{trnxId}-RS", JsonConvert.SerializeObject(rs), "SupplierOne", "Search");
                return rs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClientBookRS?> HandleBookAsync(ClientBookRQ request)
        {
            try
            {
                FileHelper.JsonFileSaveAsync($"Master-{request.TrnxId}-RQ", JsonConvert.SerializeObject(request), "SupplierOne", "Book");

                Random rnd = new();
                // Call the supplier service
                var supplierSearchRQ = new SupplierBookRQ
                {
                    ApiCredential = new ApiCredential
                    {
                        UserName = "user",
                        Password = "password",
                        Url = "http://supplierone.com",
                        ApiKey = "key"
                    },
                    ConvertionRate = 1.0m,
                    ItemCodeRef = request.ItemCodeRef,
                    Passengers = request.Passengers,
                    TrnxId = request.TrnxId
                };
                var baseUrl = configuration.GetSection("SuppliersURL:SupplierOne").Value ?? string.Empty;

                var supplierSearchRSStr = await ReqRspHelper<SupplierSearchRQ>.PostRequest(
                    supplierSearchRQ,
                    baseUrl + "/api/flight/book"
                    );

                if (string.IsNullOrEmpty(supplierSearchRSStr))
                    return null;

                var supplierSearchRS = JsonConvert.DeserializeObject<SupplierBookRS>(supplierSearchRSStr);

                // Convert the supplier response to client response

                var rs = new ClientBookRS()
                {
                    Passengers = supplierSearchRS.Passengers,
                    Status = supplierSearchRS.Status,
                    PNR = supplierSearchRS.PNR,
                    TrnxId = supplierSearchRS.TrnxId,
                    PassengerFares = supplierSearchRS.PassengerFares,
                    Directions = supplierSearchRS.Directions,
                    FlightPrice = supplierSearchRS.FlightPrice,
                    FlightNumber = supplierSearchRS.FlightNumber,
                    LastTicketingTimeLimit = supplierSearchRS.LastTicketingTimeLimit
                };


                FileHelper.JsonFileSaveAsync($"Master-{request.TrnxId}-RS", JsonConvert.SerializeObject(rs), "SupplierOne", "Book");
                return rs;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
