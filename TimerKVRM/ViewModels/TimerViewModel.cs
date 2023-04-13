using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using TimerKVRM.Models;

namespace TimerKVRM.ViewModels
{
    internal class TimerViewModel
    {
        private DispatcherTimer _timer;

        public int DisplayTime { get; set; }

        public ICommand QuestionCommand { get; }
        public ICommand StopCommand { get; }

        public TimerViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += OnTimeChanged;

            DisplayTime = 60;

            QuestionCommand = new RelayCommand(Question);
            StopCommand = new RelayCommand(StopTimer);
        }

        private void Question()
        {
            DisplayTime = 60;
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.Stop();
            DisplayTime = 60;
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            DisplayTime--;

            if (DisplayTime < 1)
            {
                StopTimer();
            }
        }
    }
}
