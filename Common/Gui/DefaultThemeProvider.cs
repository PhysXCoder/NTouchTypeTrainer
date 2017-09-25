using NTouchTypeTrainer.Interfaces.Common.Gui;
using System;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Common.Gui
{
    public class DefaultThemeProvider : IThemeProvider
    {
        public Color WindowColor { get; private set; }

        public Brush WrongTextBrush { get; private set; }

        public Brush WrongTextBackgroundBrush { get; private set; }

        public Brush ButtonBackgroundBrush { get; private set; }

        public Brush ButtonForegroundBrush { get; private set; }

        public IFont ButtonFont { get; private set; }

        public Size PaddingSize { get; private set; }

        public FontFamily TextFontFamily { get; private set; }

        public DefaultThemeProvider()
        {
            InitProperties();
        }

        private void InitProperties()
        {
            WindowColor = SystemColors.WindowColor;
            WrongTextBrush = new SolidColorBrush(Colors.Red);
            WrongTextBackgroundBrush = new SolidColorBrush(Colors.DarkCyan);
            ButtonBackgroundBrush = SystemColors.ControlBrush;
            ButtonForegroundBrush = SystemColors.ControlTextBrush;

            var defaultWindow = new Window();
            ButtonFont = new Font
            {
                Family = defaultWindow.FontFamily,
                Size = defaultWindow.FontSize + 3,
                Style = defaultWindow.FontStyle,
                Stretch = defaultWindow.FontStretch,
                Weight = defaultWindow.FontWeight
            };

            TextFontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Resources/Fonts/#Liberation Mono");

            PaddingSize = new Size(8, 5);
        }
    }
}