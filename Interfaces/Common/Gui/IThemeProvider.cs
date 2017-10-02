using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Interfaces.Common.Gui
{
    public interface IThemeProvider
    {
        Color WindowColor { get; }

        Brush WrongTextBrush { get; }

        Brush WrongTextBackgroundBrush { get; }

        Brush ButtonBackgroundBrush { get; }

        Brush ButtonForegroundBrush { get; }

        IFont ButtonFont { get; }

        FontFamily TextFontFamily { get; }

        double TextFontSize { get; }

        Size PaddingSize { get; }
    }
}