using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Helper
{
    public abstract class ReqRspHelper<T> where T : class
    {
        public static async Task<string> PostRequest<T>(T requestJson, string url, string accessToken = "", int timeout = 600)
        {
            try
            {
                using var httpClient = new HttpClient();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                httpClient.Timeout = TimeSpan.FromSeconds(timeout);
                StringContent content = new StringContent(JsonConvert.SerializeObject(requestJson), Encoding.UTF8, "application/json");
                using var response = await httpClient.PostAsync(url, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                return apiResponse;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

        }
    }
}
