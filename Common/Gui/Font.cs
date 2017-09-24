using NTouchTypeTrainer.Interfaces.Common.Gui;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Common.Gui
{
    public class Font : IFont
    {
        public FontFamily Family { get; set; }
        public double Size { get; set; }
        public FontStyle Style { get; set; }
        public FontStretch Stretch { get; set; }
        public FontWeight Weight { get; set; }
    }
}