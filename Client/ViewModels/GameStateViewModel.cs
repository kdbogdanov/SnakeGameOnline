using Prism.Events;
using Prism.Mvvm;
using Client.Models;
using Client.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Client.ViewModels.Events;

namespace Client.ViewModels
{
    public class GameStateViewModel : BindableBase, INotifyPropertyChanged
    {
        private HttpClient _client;
        private readonly IEventAggregator _eventAggregator;
        private readonly RoundInfo _roundInfo;
        private GameStateResponse _gameState;
        private readonly DirectorRequest _directorRequest;
        private string _nickname;
        private ObservableCollection<ObservableCollection<CellType>> _cells;

        private async void GameStart(GameStartInfo gameStartInfo)
        {
            if (!gameStartInfo.IsStarted)
                return;

            _nickname = gameStartInfo.Nickname;
            _roundInfo.Players = new ObservableCollection<User>();
            _client = gameStartInfo.Client;
            _gameState = await _client.GetGameStateAsync();
            _directorRequest.Direction = "Left";
            _client.PostSnakeDirection(_directorRequest);

            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Tick += new EventHandler(GameStep);
            timer1.Interval = TimeSpan.FromMilliseconds(_gameState.TurnTimeMilleseconds);
            timer1.Start();
        }
        private async void GameStep(object sender, EventArgs e)
        {
            _gameState = await _client.GetGameStateAsync();
            _roundInfo.Players.Clear();
            UpdateCells(_gameState, _roundInfo.Round != _gameState.RoundNumber);
            _roundInfo.Round = _gameState.RoundNumber;
            _eventAggregator.GetEvent<PubSubEvent<RoundInfo>>().Publish(_roundInfo);
        }


        private async void Post(string direction)
        {
            await Task.Delay(_gameState.TimeUntilNextTurnMilleseconds);
            _directorRequest.Direction = direction;
            _client.PostSnakeDirection(_directorRequest);
        }
        private void UpdateCells(GameStateResponse gameState, bool full = false)
        {
            var height = 2 + gameState.GameBoardSize.Height;
            var width = 2 + gameState.GameBoardSize.Width;

            if (_cells is null || full && (height != _cells.Count() || width != _cells.Count()))
            {
                _cells = new ObservableCollection<ObservableCollection<CellType>>();
                for (int i = 0; i < height; ++i)
                {
                    ObservableCollection<CellType> Row = new ObservableCollection<CellType>();
                    for (int j = 0; j < width; ++j)
                    {
                        if (j == 0 || j + 1 == width || i == 0 || i + 1 == height)
                            Row.Add(CellType.Wall);
                        else
                            Row.Add(CellType.Free);
                    }
                    _cells.Add(Row);
                }
            }
            else
                for (int i = 0; i < height; ++i)
                    for (int j = 0; j < width; ++j)
                        if (_cells[i][j] != CellType.Wall)
                            _cells[i][j] = CellType.Free;

            foreach (Point2D food in gameState.Food ?? Enumerable.Empty<Point2D>())
                _cells[food.Y + 1][food.X + 1] = CellType.Food;

            foreach (PlayerState snake in gameState.Players ?? Enumerable.Empty<PlayerState>())
            {
                _roundInfo.Players.Add(new User(snake.Name, snake.Snake == null ? 0 : snake.Snake.Count));
                foreach (Point2D point in snake.Snake ?? Enumerable.Empty<Point2D>())
                    if (snake.Name == _nickname)
                        _cells[point.Y + 1][point.X + 1] = snake.IsSpawnPtotected ? CellType.MyProtectedSnake
                        : CellType.MySnake;
                    else
                        _cells[point.Y + 1][point.X + 1] = snake.IsSpawnPtotected ? CellType.EnemyProtectedSnake
                        : CellType.EnemySnake;
            }

            if (full)
                foreach (Rectangle2D wall in gameState.Walls ?? Enumerable.Empty<Rectangle2D>())
                    for (int i = 0; i < wall.Height; ++i)
                        for (int j = 0; j < wall.Width; ++j)
                            _cells[wall.Y + i + 1][wall.X + j + 1] = CellType.Wall;

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(GridCells)));
        }

        public GameStateViewModel(IEventAggregator eventAggregator)
        {
            _directorRequest = new DirectorRequest("", Config.Token);
            _roundInfo = new RoundInfo();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PubSubEvent<GameStartInfo>>().Subscribe(GameStart);
            _eventAggregator.GetEvent<DirectionEvent>().Subscribe(Post);
        }

        public new event PropertyChangedEventHandler PropertyChanged = (a, s) => { };

        public ObservableCollection<ObservableCollection<CellType>> GridCells
        {
            get { return _cells; }
            set
            {
                _cells = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(GridCells)));
            }
        }
    }
}

