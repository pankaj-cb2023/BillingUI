using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BillingUI.Settings
{
    public class CustomClient
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _headers;

        public CustomClient(string baseUrl, Dictionary<string, string> headers)
        {
            _baseUrl = baseUrl;
            _headers = headers;
        }

        public async Task<WebServiceResponse> DownloadDataAsync(string endpoint, CustomClientRequest request)
        {
            using (var client = new HttpClient())
            {
                // Add headers
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
              
                var queryString = new FormUrlEncodedContent(request.QueryStringList!).ReadAsStringAsync().Result;
               
                string fullUrl = $"{_baseUrl}{endpoint}?{queryString}";
               
                var response = await client.GetAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return new WebServiceResponse
                    {
                        IsSuccess = false,
                        StatusCode = response.StatusCode,
                        ErrorMessage = $"Error: {response.ReasonPhrase}"
                    };
                }
                var responseData = await response.Content.ReadAsStringAsync();              

                return new WebServiceResponse
                {
                    IsSuccess = true,
                    StatusCode = response.StatusCode,
                    Data = responseData
                };
            }
        }


    }
}
