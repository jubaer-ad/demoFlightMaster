using System.Text;
using System.Text.Json;
using Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        [HttpGet("stream")]
        public async Task GetChunkedData()
        {
            // Configure the response for chunked transfer
            var response = HttpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "application/json";
            response.Headers.Add("Transfer-Encoding", "chunked");
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");

            // Create a list of API endpoints we'll be calling
            // In a real scenario, these would be actual external API endpoints
            var endpoints = new List<(string url, int delayMs)>
            {
                ("https://jsonplaceholder.typicode.com/posts/1", 2000),
                ("https://jsonplaceholder.typicode.com/posts/2", 1000),
                ("https://jsonplaceholder.typicode.com/posts/3", 3000),
                ("https://jsonplaceholder.typicode.com/posts/4", 500),
                ("https://jsonplaceholder.typicode.com/posts/5", 1500)
            };

            // Start the JSON array
            await WriteChunked(response, "[\n");

            // Create tasks for all API calls
            var tasks = new List<Task<(ApiResponse response, int index)>>();

            for (int i = 0; i < endpoints.Count; i++)
            {
                int capturedIndex = i; // Capture the loop variable
                var (url, delay) = endpoints[i];

                // Start all requests asynchronously
                tasks.Add(FetchApiDataAsync(httpClientFactory, url, delay, capturedIndex));
            }

            // Process results as they complete
            bool isFirst = true;
            while (tasks.Count > 0)
            {
                // Wait for any task to complete
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);

                var (result, index) = await completedTask;

                // Write comma for all but the first item
                if (!isFirst)
                {
                    await WriteChunked(response, ",\n");
                }
                else
                {
                    isFirst = false;
                }

                // Serialize and write the result
                string json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await WriteChunked(response, json);
            }

            // Close the JSON array
            await WriteChunked(response, "\n]");
            await WriteChunked(response, ""); // Send the final empty chunk
        }

        // Helper method to simulate an API call with a specific delay
        private async Task<(ApiResponse, int)> FetchApiDataAsync(IHttpClientFactory httpClientFactory, string url, int delayMs, int index, int waitTime = 0)
        {
            Random rnd = new();
            var client = httpClientFactory.CreateClient();
            var startTime = DateTime.Now;
            waitTime = rnd.Next(1000, 10000);
            await Task.Delay(waitTime);
            // Make the real HTTP request
            var httpResponse = await client.GetAsync(url);
            
            // Simulate additional processing time
            await Task.Delay(delayMs);
            
            var content = await httpResponse.Content.ReadAsStringAsync();
            var processingTime = DateTime.Now - startTime;
            
            // Create response object
            var apiResponse = new ApiResponse
            {
                Id = index + 1,
                Source = url,
                Data = content.Length > 100 ? content.Substring(0, 100) + "..." : content,
                Timestamp = DateTime.Now,
                ProcessingTime = processingTime
            };
            
            return (apiResponse, index);
        }

        // Endpoint that provides a simple HTML page to test the chunked response
        [HttpGet("test")]
        public IActionResult GetTestPage()
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Resource", "chunked-response-viewer.html");

            if (!System.IO.File.Exists(filePath))
            {
                return Content("File not found", "text/plain", System.Text.Encoding.UTF8); // Or return a 404
            }

            var htmlContent = System.IO.File.ReadAllText(filePath);

            return Content(htmlContent, "text/html", System.Text.Encoding.UTF8);
        }
        private async Task WriteChunked(HttpResponse response, string data)
        {
            string chunk = $"{Convert.ToString(data.Length, 16)}\r\n{data}\r\n";
            await response.WriteAsync(chunk);
            await response.Body.FlushAsync();
        }

    }
}
