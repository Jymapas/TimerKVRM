using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TimerKVRM.ViewModels
{
    internal class TimerViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;
        private int _displayTime;
        private SolidColorBrush _displayColour;
        public int QuestionTime { get; private set; }
        public int DoubletTime { get; private set; }
        public int BlitzTime { get; private set; }

        public int DisplayTime
        {
            get => _displayTime;
            private set
            {
                _displayTime = value;
                OnPropertyChanged();
            }
        }

        private bool _isQuestionEnabled, _isDoubletEnabled, _isBlitzEnabled;

        public bool IsQuestionEnabled
        {
            get => _isQuestionEnabled;
            set
            {
                _isQuestionEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsDoubletEnabled
        {
            get => _isDoubletEnabled;
            set
            {
                _isDoubletEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsBlitzEnabled
        {
            get => _isBlitzEnabled;
            set
            {
                _isBlitzEnabled = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush DisplayColour
        {
            get => _displayColour;
            set
            {
                _displayColour = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand QuestionCommand { get; }
        public ICommand DoubletCommand { get; }
        public ICommand BlitzCommand { get; }
        public ICommand StopCommand { get; }

        public TimerViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            QuestionCommand = new RelayCommand(Question);
            DoubletCommand = new RelayCommand(Doublet);
            BlitzCommand = new RelayCommand(Blitz);
            StopCommand = new RelayCommand(StopTimer);

            ClearView();
        }

        private void ClearView()
        {
            DisplayTime = 60;
            ButtonStatesChange(true);
            QuestionTime = 60;
            DoubletTime = 30;
            BlitzTime = 20;
            DisplayColour = Brushes.Black;
        }

        private void SetCountdownView()
        {
            DisplayTime = 10;
            DisplayColour = Brushes.Red;
            _timer.Tick += FinalCountdownTick;
        }

        private void ButtonStatesChange(bool isEnable = false)
        {
            IsQuestionEnabled = IsDoubletEnabled = IsBlitzEnabled = isEnable;
        }

        private void Question()
        {
            ButtonStatesChange();
            _timer.Tick += QuestionTick;
            _timer.Start();
        }

        private void Doublet()
        {
            ButtonStatesChange();
            _timer.Tick += DoubletTick;
            _timer.Start();
        }

        private void Blitz()
        {
            ButtonStatesChange();
            _timer.Tick += BlitzTick;
            _timer.Start();
        }


        private void StopTimer()
        {
            _timer.Tick -= QuestionTick;
            _timer.Tick -= DoubletTick;
            _timer.Tick -= BlitzTick;
            _timer.Tick -= FinalCountdownTick;
            _timer.Stop();
            ClearView();
        }

        private void QuestionTick(object sender, EventArgs e)
        {
            DisplayTime--;

            if (DisplayTime > 0) return;

            _timer.Tick -= QuestionTick;
            SetCountdownView();
        }

        private void DoubletTick(object sender, EventArgs e)
        {
            DisplayTime--;
            DoubletTime--;

            if(DoubletTime > 0) return;
            _timer.Tick -= DoubletTick;
            if (DisplayTime == 0)
            {
                SetCountdownView();
                return;
            }

            IsDoubletEnabled = true;
            DoubletTime = 30;
        }

        private void BlitzTick(object sender, EventArgs e)
        {
            DisplayTime--;
            BlitzTime--;

            if(BlitzTime > 0) return;
            _timer.Tick -= DoubletTick;
            if (DisplayTime == 0)
            {
                SetCountdownView();
                return;
            }

            IsBlitzEnabled = true;
            BlitzTime = 20;
        }

        private void FinalCountdownTick(object sender, EventArgs e)
        {
            DisplayTime--;
            if (DisplayTime > 0) return;
            StopTimer();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
