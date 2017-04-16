using System;
using System.Threading;

namespace NTouchTypeTrainer.Common.Threading
{
    public class ActionDelayer
    {
        private Action _action;

        private readonly Timer _timer;
        private readonly MonitorSynchronizationProvider _synchronizationProvider;

        public ActionDelayer()
        {
            _timer = new Timer(TimerCallback);
            _synchronizationProvider = new MonitorSynchronizationProvider();
        }

        public void Delay(Action action, TimeSpan delay)
        {
            _synchronizationProvider.Execute(() =>
            {
                Abort();

                _action = action ?? throw new ArgumentNullException(nameof(action));

                _timer.Change(
                    dueTime: TimeSpan.FromMilliseconds(0),
                    period: delay);
            });
        }

        public void Abort()
        {
            _synchronizationProvider.Execute(() =>
            {
                _timer?.Change(-1, -1);
            });
        }

        private void TimerCallback(object state)
        {
            Abort();
            _action?.Invoke();
        }
    }
}