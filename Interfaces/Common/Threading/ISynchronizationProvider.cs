using System;

namespace NTouchTypeTrainer.Interfaces.Common.Threading
{
    public interface ISynchronizationProvider
    {
        void Execute(Action method);

        void Execute(Action method, TimeSpan timeout);
    }
}