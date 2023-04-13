using System;
using System.Windows.Threading;

namespace TimerKVRM.Models
{
    internal class TimerModel
    {
        private DispatcherTimer _timer;
        private int _displayTime;

        public TimerModel()
        {
            _timer = new DispatcherTimer();
        }

        public void Start(int seconds)
        {
            _displayTime = seconds;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
