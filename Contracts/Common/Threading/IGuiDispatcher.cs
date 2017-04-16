﻿using System;

namespace NTouchTypeTrainer.Contracts.Common.Threading
{
    public interface IGuiDispatcher
    {
        void ExecuteOnGuiThread(Action action);
        void BeginExecuteOnGuiThread(Action action);
    }
}