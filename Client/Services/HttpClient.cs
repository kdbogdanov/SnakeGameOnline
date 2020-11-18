using RestSharp;
using Client.Models;
using System.Threading.Tasks;

namespace Client.Services
{
    public class HttpClient
    {
        private readonly RestClient _httpClient;

        public HttpClient() =>
            _httpClient = new RestClient(Config.Uri);

        public void PostSnakeDirection(DirectorRequest directorRequest)
        {
            var request = new RestRequest("direction", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(new
            {
                direction = directorRequest.Direction,
                token = directorRequest.Token
            });
            _httpClient.Execute(request);
        }

        public async Task<string> GetNameAsync()
        {
            var request = new RestRequest("name?token=" + Config.Token, Method.GET);
            var response = await _httpClient.ExecuteGetAsync<NameResponse>(request);
            return response.Data.Name;
        }

        public async Task<GameStateResponse> GetGameStateAsync()
        {
            var request = new RestRequest("gameboard?token=" + Config.Token, Method.GET);
            var response = await _httpClient.ExecuteGetAsync<GameStateResponse>(request);
            return response.Data;
        }
    }
}
