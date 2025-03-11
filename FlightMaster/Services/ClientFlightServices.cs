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
    }
}
