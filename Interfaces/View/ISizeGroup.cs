using System.Windows;

namespace NTouchTypeTrainer.Interfaces.View
{
    public interface ISizeGroup
    {
        Size Size { get; }

        double RequestMinWidth(object requester, double necessaryWidth);

        double RequestMinHeight(object requester, double necessaryHeight);

        Size RequestMinSize(object requester, Size necessarySize);

        void ClearRequests(object requester);
    }
}
