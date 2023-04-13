using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace TimerKVRM.ViewModels
{
    internal class TimerViewModel : INotifyPropertyChanged
    {
        readonly DispatcherTimer _timer;
        private int _displayTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public int DisplayTime
        {
            get => _displayTime;
            private set
            {
                _displayTime = value;
                OnPropertyChanged();
            }
        } //set{_price = value;RaisePropertyChanged(() => Price);}

        public ICommand QuestionCommand { get; }
        public ICommand StopCommand { get; }

        public TimerViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            QuestionCommand = new RelayCommand(Question);
            StopCommand = new RelayCommand(StopTimer);
        }

        private void Question()
        {
            DisplayTime = 60;

            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void StopTimer()
        {_timer.Stop();
            DisplayTime = 60;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DisplayTime--;

            if (DisplayTime > 0) return;

            StopTimer();
            _timer.Tick -= Timer_Tick;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
