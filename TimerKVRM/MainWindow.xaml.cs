using System;
using System.Collections.Generic;
using System.Linq;
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
        static int counter, blitzCounter = 20, duplCounter = 30;
        DispatcherTimer dt = new();
        public MainWindow()
        {
            InitializeComponent();
            dt.Interval = TimeSpan.FromSeconds(1);
            Clear();
        }

        private void Question_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            dt.Tick += Dt_Tick;
            dt.Start();
        }
        private void Dupl_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            dt.Tick += Dupl_Tick;
            dt.Start();
        }
        private void Blitz_Click(object sender, RoutedEventArgs e)
        {
            Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = false;
            dt.Tick += Blitz_Tick;
            dt.Start();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        private void Dt_Tick(object? sender, EventArgs e)
        {
            counter--;
            Timer.Content = counter.ToString();
            if (counter == 0)
            {
                dt.Tick -= Dt_Tick;
                Clear(true);
                dt.Tick += FinalCountdown_Tick;
            }
        }
        private void Dupl_Tick(object? sender, EventArgs e)
        {
            counter--;
            duplCounter--;
            Timer.Content = counter.ToString();
            if (duplCounter == 0)
            {
                dt.Tick -= Dupl_Tick;
                if (counter == 0)
                {
                    Clear(true);
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
            if (blitzCounter == 0)
            {
                dt.Tick -= Blitz_Tick;
                if (counter == 0)
                {
                    Clear(true);
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
            dt.Tick -= FinalCountdown_Tick;
            dt.Tick -= Dupl_Tick;

            Clear();
        }
        private void Clear(bool isMinute = false)
        {
            if (isMinute)
            {
                counter = 10;
                Timer.Content = ten;
            }
            else
            {
                counter = 60;
                blitzCounter = 20;
                duplCounter = 30;
                Timer.Content = sixty;
                Question.IsEnabled = Dupl.IsEnabled = Blitz.IsEnabled = StopButton.IsEnabled = true;
            }
        }
    }
}
