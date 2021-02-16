using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Armut.Api.FunctionalTests.Extensions
{
    internal static class HttpContentExtensions
    {
        internal static async Task<TModel> GetAsync<TModel>(this HttpContent content)
        {
            var jsonSerializerOptions = new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            string json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TModel>(json, jsonSerializerOptions);
        }
    }

    internal static class HttpClientExtensions
    {
        internal static Task<HttpResponseMessage> PostAsync<TModel>(this HttpClient client, string requestUri, TModel model)
        {
            string json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return client.PostAsync(requestUri, content);
        }
    }
}
