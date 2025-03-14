﻿using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupplierOne.Services;

namespace SupplierOne.Controllers
{
    [Route("api/flight")]
    [ApiController]
    public class FlightController(FlightService flightService) : ControllerBase
    {
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromBody] SupplierSearchRQ request)
        {
            var rs = await flightService.HandleSearchAsync(request);
            return Ok(rs);
        }

        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> Book([FromBody] SupplierBookRQ request)
        {
            var rs = await flightService.HandleBookAsync(request);
            return Ok(rs);
        }

    }
}
