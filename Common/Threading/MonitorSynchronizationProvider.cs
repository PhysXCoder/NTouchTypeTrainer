using NLog;
using NTouchTypeTrainer.Common.Exceptions;
using NTouchTypeTrainer.Interfaces.Common.Threading;
using System;
using System.Threading;
using static NTouchTypeTrainer.Common.Logging.LoggingExtensions;

namespace NTouchTypeTrainer.Common.Threading
{
    public class MonitorSynchronizationProvider : ISynchronizationProvider
    {
        private readonly object _synchronizationToken;

        private readonly ILogger _logger;

        private static readonly TimeSpan DeadlockTimeout = TimeSpan.FromSeconds(25);

        public MonitorSynchronizationProvider()
        {
            _synchronizationToken = new object();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Execute(Action method) => Execute(method, DeadlockTimeout);

        public void Execute(Action method, TimeSpan timeout)
        {
            if (method == null)
            {
                return;
            }

            _logger.Trace(Enter(GetLogId()));
            if (!Monitor.TryEnter(_synchronizationToken, timeout))
            {
                throw new DeadlockException();
            }

            try
            {
                _logger.Trace("Invoking method " + GetCallerIdText(GetLogId()));
                method.Invoke();
            }
            finally
            {
                Monitor.Exit(_synchronizationToken);
            }

            _logger.Trace(Leave(GetLogId()));
        }

        private string GetLogId() => _synchronizationToken.GetHashCode().ToString();
    }
}