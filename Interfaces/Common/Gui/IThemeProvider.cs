using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Interfaces.Common.Gui
{
    public interface IThemeProvider
    {
        Color Color { get; }

        Brush BackgroundBrush { get; }

        Brush ForegroundBrush { get; }

        IFont ButtonFont { get; }

        FontFamily TextFontFamily { get; }

        Size PaddingSize { get; }
    }
}