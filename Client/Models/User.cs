namespace Client.Models
{
    public class User
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public User(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
