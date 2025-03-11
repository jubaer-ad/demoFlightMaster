using Core.Models;
using FlightMaster.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientFlightController(ClientFlightServices clientFlightServices) : ControllerBase
    {
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromBody] ClientSearchRQ request)
        {
            var rs = await clientFlightServices.HandleSearchAsync(request);
            return Ok(rs);
        }

        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> Books([FromBody] ClientBookRQ request)
        {
            var rs = await clientFlightServices.HandleBookAsync(request);
            return Ok(rs);
        }
    }
}
