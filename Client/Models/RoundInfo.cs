using System.Collections.ObjectModel;

namespace Client.Models
{
    public class RoundInfo
    {
        public ObservableCollection<User> Players { get; set; }
        public int Round { get; set; }

    }
}
