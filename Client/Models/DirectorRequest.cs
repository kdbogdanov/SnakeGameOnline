namespace Client.Models
{
    public class DirectorRequest
    {
        public string Direction { get; set; }

        public string Token { get; set; }

        public DirectorRequest(string direction, string token)
        {
            Direction = direction;
            Token = token;
        }
    }
}
