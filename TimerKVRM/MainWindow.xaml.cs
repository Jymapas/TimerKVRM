using System;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace TimerKVRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string Sixty = "60", Ten = "10";
        private static int _counter, _blitzCounter, _doubletCounter;
        readonly DispatcherTimer dt = new DispatcherTimer();
        private readonly SoundPlayer _fc = new SoundPlayer(Properties.Resources.finialCountdown);
        private readonly SoundPlayer _start = new SoundPlayer(Properties.Resources.start);
        private readonly SoundPlayer _bBrake = new SoundPlayer(Properties.Resources.bBreak);
        private readonly SoundPlayer _tenSec = new SoundPlayer(Properties.Resources.tenSeconds);

        public MainWindow()
        {
            InitializeComponent();
            dt.Interval = TimeSpan.FromSeconds(1);
            Clear();
        }

        private void Question_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            _start.Play();
            dt.Tick += Dt_Tick;
            dt.Start();
        }
        private void Dupl_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            _start.Play();
            dt.Tick += Dupl_Tick;
            dt.Start();
        }
        private void Blitz_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            _start.Play();
            dt.Tick += Blitz_Tick;
            dt.Start();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _fc.Stop();
            Stop();
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            _counter--;
            Timer.Content = _counter.ToString();
            if (_counter == 11) TenSecondsPlay();
            if (_counter == 0)
            {
                dt.Tick -= Dt_Tick;
                Clear(true);
                _fc.Play();
                dt.Tick += FinalCountdown_Tick;
            }
        }
        private void Dupl_Tick(object sender, EventArgs e)
        {
            _counter--;
            _doubletCounter--;
            Timer.Content = _counter.ToString();
            if (_counter == 11) TenSecondsPlay();
            if (_doubletCounter == 0)
            {
                dt.Tick -= Dupl_Tick;
                if (_counter == 0)
                {
                    Clear(true);
                    _fc.Play();
                    dt.Tick += FinalCountdown_Tick;
                }
                else
                {
                    _bBrake.Play();
                    Dupl.IsEnabled = true;
                    _doubletCounter = 30;
                }
            }
        }
        private void Blitz_Tick(object sender, EventArgs e)
        {
            _counter--;
            _blitzCounter--;
            Timer.Content = _counter.ToString();
            if (_counter == 11) TenSecondsPlay();
            if (_blitzCounter == 0)
            {
                dt.Tick -= Blitz_Tick;
                if (_counter == 0)
                {
                    Clear(true);
                    _fc.Play();
                    dt.Tick += FinalCountdown_Tick;
                }
                else
                {
                    _bBrake.Play();
                    Blitz.IsEnabled = true;
                    _blitzCounter = 20;
                }
            }
        }
        private void FinalCountdown_Tick(object sender, EventArgs e)
        {
            _counter--;
            Timer.Content = _counter.ToString();
            if (_counter == 0)
            {
                Stop();
            }
        }

        private void Stop(bool isMinute = false)
        {
            dt.Stop();
            dt.Tick -= Dt_Tick;
            dt.Tick -= Dupl_Tick;
            dt.Tick -= Blitz_Tick;
            dt.Tick -= FinalCountdown_Tick;
            Clear();
        }
        private void Clear(bool isMinute = false)
        {
            if (isMinute)
            {
                _counter = 10;
                Timer.Content = Ten;
                Timer.Foreground = Brushes.Red;
            }
            else
            {
                _counter = 60;
                _blitzCounter = 20;
                _doubletCounter = 30;
                Timer.Content = Sixty;
                Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = StopButton.IsEnabled = true;
                Timer.Foreground = Brushes.Black;
            }
        }
        private void TenSecondsPlay()
        {
            _tenSec.Play();
        }
    }
}
