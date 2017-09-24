using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Interfaces.Common.Gui
{
    public interface IFont
    {
        FontFamily Family { get; }

        double Size { get; }

        FontStyle Style { get; }

        FontStretch Stretch { get; }

        FontWeight Weight { get; }
    }
}