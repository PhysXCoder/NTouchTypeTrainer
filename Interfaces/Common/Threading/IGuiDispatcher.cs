using System;

namespace NTouchTypeTrainer.Interfaces.Common.Threading
{
    public interface IGuiDispatcher
    {
        void ExecuteOnGuiThread(Action action);
        void BeginExecuteOnGuiThread(Action action);
    }
}