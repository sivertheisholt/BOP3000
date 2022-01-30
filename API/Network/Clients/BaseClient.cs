using System.Text.Json;

namespace API.Network.Clients
{
    abstract public class BaseClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        protected BaseClient(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected JsonSerializerOptions JsonSerializerOptions { get { return _jsonSerializerOptions; } }

        protected HttpClient Client { get { return _client; } }
    }
}