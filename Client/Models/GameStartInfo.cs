using Client.Services;

namespace Client.Models
{
    public class GameStartInfo
    {
        public bool IsStarted { get; set; }
        public string Nickname { get; set; }
        public HttpClient Client { get; set; }
    }
}
