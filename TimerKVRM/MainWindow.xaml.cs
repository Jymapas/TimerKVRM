using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimerKVRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string sixty = "60", ten = "10";
        static int counter, blitzCounter, duplCounter;
        DispatcherTimer dt = new();
        SoundPlayer fc = new(Properties.Resources.finialCountdown);
        SoundPlayer start = new(Properties.Resources.start);
        SoundPlayer bBrake = new(Properties.Resources.bBreak);
        SoundPlayer tenSec = new(Properties.Resources.tenSeconds);

        public MainWindow()
        {
            InitializeComponent();
            dt.Interval = TimeSpan.FromSeconds(1);
            Clear();
        }

        private void Question_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            start.Play();
            dt.Tick += Dt_Tick;
            dt.Start();
        }
        private void Dupl_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            start.Play();
            dt.Tick += Dupl_Tick;
            dt.Start();
        }
        private void Blitz_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            start.Play();
            dt.Tick += Blitz_Tick;
            dt.Start();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            fc.Stop();
            Stop();
        }
        private void Dt_Tick(object? sender, EventArgs e)
        {
            counter--;
            Timer.Content = counter.ToString();
            if (counter == 11) TenSecondsPlay();
            if (counter == 0)
            {
                dt.Tick -= Dt_Tick;
                Clear(true);
                fc.Play();
                dt.Tick += FinalCountdown_Tick;
            }
        }
        private void Dupl_Tick(object? sender, EventArgs e)
        {
            counter--;
            duplCounter--;
            Timer.Content = counter.ToString();
            if (counter == 11) TenSecondsPlay();
            if (duplCounter == 0)
            {
                dt.Tick -= Dupl_Tick;
                bBrake.Play();
                if (counter == 0)
                {
                    Clear(true);
                    fc.Play();
                    dt.Tick += FinalCountdown_Tick;
                }
                else
                {
                    Dupl.IsEnabled = true;
                    duplCounter = 30;
                }
            }
        }
        private void Blitz_Tick(object? sender, EventArgs e)
        {
            counter--;
            blitzCounter--;
            Timer.Content = counter.ToString();
            if (counter == 11) TenSecondsPlay();
            if (blitzCounter == 0)
            {
                dt.Tick -= Blitz_Tick;
                bBrake.Play();
                if (counter == 0)
                {
                    Clear(true);
                    fc.Play();
                    dt.Tick += FinalCountdown_Tick;
                }
                else
                {
                    Blitz.IsEnabled = true;
                    blitzCounter = 20;
                }
            }
        }
        private void FinalCountdown_Tick(object? sender, EventArgs e)
        {
            counter--;
            Timer.Content = counter.ToString();
            if (counter == 0)
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
                counter = 10;
                Timer.Content = ten;
                Timer.Foreground = Brushes.Red;
            }
            else
            {
                counter = 60;
                blitzCounter = 20;
                duplCounter = 30;
                Timer.Content = sixty;
                Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = StopButton.IsEnabled = true;
                Timer.Foreground = Brushes.Black;
            }
        }
        private void TenSecondsPlay()
        {
            tenSec.Play();
        }
    }
}
