using Client.Models;
using Client.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly HttpClient _client;
        private readonly IEventAggregator _eventAggregator;

        private async void StartGame()
        {
            string nickname = await _client.GetNameAsync();
            _eventAggregator.GetEvent<PubSubEvent<string>>().Publish(nickname);
            GameStartInfo gameStartInfo = new GameStartInfo
            {
                IsStarted = true,
                Nickname = nickname,
                Client = _client
            };
            _eventAggregator.GetEvent<PubSubEvent<GameStartInfo>>().Publish(gameStartInfo);
        }

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _client = new HttpClient();
            _eventAggregator = eventAggregator;
            StartCommand = new DelegateCommand(StartGame);
        }

        public new event PropertyChangedEventHandler PropertyChanged = (a, s) => { };

        public DelegateCommand StartCommand { get; set; }
    }
}
