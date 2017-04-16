using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NTouchTypeTrainer.Common.GuiExtensions
{
    public static class EventExtensions
    {
        public static void RaiseEvent(
            this PropertyChangedEventHandler handler,
            object sender,
            PropertyChangedEventArgs e)
        {
            handler?.Invoke(sender, e);
        }

        public static void RaiseEvent(
            this PropertyChangedEventHandler handler,
            object sender,
            [CallerMemberName] string propertyName = null)
        {
            handler?.RaiseEvent(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}