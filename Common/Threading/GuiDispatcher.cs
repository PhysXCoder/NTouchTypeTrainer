using System;
using System.Windows.Threading;
using NTouchTypeTrainer.Contracts.Common.Threading;

namespace NTouchTypeTrainer.Common.Threading
{    
    public class GuiDispatcher : IGuiDispatcher
    {
        // ToDo: This is a windows specific implementation. Should be changed. (Eto.Forms must be extended?)
        private Dispatcher _dispatcher;

        public GuiDispatcher()
        {
            // ToDo: This is a windows specific implementation. Should be changed. (Eto.Forms must be extended?)
            SetCurrentDispatcher();
        }

        public void SetCurrentDispatcher()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void ExecuteOnGuiThread(Action action)
        {
            if (action != null)
            {
                _dispatcher.Invoke(action);
            }
        }

        public void BeginExecuteOnGuiThread(Action action)
        {
            if (action != null)
            {
                _dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
        }
    }
}