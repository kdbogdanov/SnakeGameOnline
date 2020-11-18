using Client.Models;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class InfoBarViewModel : BindableBase, INotifyPropertyChanged
    {
        private string _nickname;
        private int _round;
        private ObservableCollection<User> _players;
        private IEventAggregator _eventAggregator;

        public string Nickname
        {
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Nickname)));
            }
        }

        public int Round
        {
            get
            {
                return _round;
            }
            set
            {
                _round = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Round)));
            }
        }

        public ObservableCollection<User> Players
        {
            get
            {
                return _players;
            }
            set
            {
                _players = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Players)));
            }
        }

        public InfoBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(UpdateNickname);
            _eventAggregator.GetEvent<PubSubEvent<RoundInfo>>().Subscribe(UpdateRoundInfo);
        }

        public void UpdateNickname(string nickname)
        {
            Nickname = nickname;
        }

        public void UpdateRoundInfo(RoundInfo roundInfo)
        {
            Round = roundInfo.Round;
            Players = roundInfo.Players;
        }

        public new event PropertyChangedEventHandler PropertyChanged = (a, s) => { };
    }
}
