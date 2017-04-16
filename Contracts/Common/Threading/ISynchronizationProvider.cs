using System;

namespace NTouchTypeTrainer.Contracts.Common.Threading
{
    public interface ISynchronizationProvider
    {
        void Execute(Action method);

        void Execute(Action method, TimeSpan timeout);
    }
}