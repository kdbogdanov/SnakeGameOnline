using Client.ViewModels.Base;
using Client.ViewModels.Events;
using Prism.Events;
using Prism.Mvvm;
using System.ComponentModel;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ICommand _leftCommand;
        private ICommand _rightCommand;
        private ICommand _upCommand;
        private string _title;
        private ICommand _downCommand;

        public new event PropertyChangedEventHandler PropertyChanged = (a, s) => { };

        public IEventAggregator _eventAggregator;
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _title = "Snake Game Online";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        public ICommand LeftCommand
        {
            get
            {
                return _leftCommand ??= new ActionCommand(() =>
                    {
                        _eventAggregator.GetEvent<DirectionEvent>().Publish("Left");
                    });
            }
        }
        public ICommand RightCommand
        {
            get
            {
                return _rightCommand ??= new ActionCommand(() =>
                {
                    _eventAggregator.GetEvent<DirectionEvent>().Publish("Right");
                });
            }
        }

        public ICommand UpCommand
        {
            get
            {
                return _upCommand ??= new ActionCommand(() =>
                    {
                        _eventAggregator.GetEvent<DirectionEvent>().Publish("Up");
                    });
            }
        }

        public ICommand DownCommand
        {
            get
            {
                return _downCommand ??= new ActionCommand(() =>
                    {
                        _eventAggregator.GetEvent<DirectionEvent>().Publish("Down");
                    });
            }
        }
    }
}
