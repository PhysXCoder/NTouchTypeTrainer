using System;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Common.Gui
{
    public class DefaultThemeProvider : IThemeProvider
    {
        public Color Color { get; private set; }

        public Brush BackgroundBrush { get; private set; }

        public Brush ForegroundBrush { get; private set; }

        public IFont ButtonFont { get; private set; }

        public Size PaddingSize { get; private set; }

        public FontFamily TextFontFamily { get; private set; }

        public DefaultThemeProvider()
        {
            InitProperties();
        }

        private void InitProperties()
        {
            Color = SystemColors.WindowColor;
            BackgroundBrush = SystemColors.ControlBrush;
            ForegroundBrush = SystemColors.ControlTextBrush;

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