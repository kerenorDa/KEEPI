using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Keepi.Client.Communication
{
    public class HttpService : IHttpService
    {
        private const string JSON = "application/json";

        private readonly HttpClient httpClient;

        private readonly JsonSerializerOptions defaultSerializerOptions;

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            defaultSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public async Task<HttpResponseContainer<T>> Get<T>(string url)
        {
            var httpResponse = await httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(httpResponse);
                return new HttpResponseContainer<T>(response, true, httpResponse);
            }
            else
            {
                return new HttpResponseContainer<T>(default, false, httpResponse);
            }
        }

        public async Task<HttpResponseContainer<object>> Post<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, JSON);
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseContainer<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseContainer<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, JSON);
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response);
                return new HttpResponseContainer<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseContainer<TResponse>(default, false, response);
            }
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, defaultSerializerOptions);
        }
    }
}
