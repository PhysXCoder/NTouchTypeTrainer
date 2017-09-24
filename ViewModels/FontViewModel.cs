using NTouchTypeTrainer.Interfaces.Common.Gui;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.ViewModels
{
    public class FontViewModel : BaseViewModel, IFont
    {
        private FontFamily _family;
        private FontWeight _weight;
        private FontStretch _stretch;
        private double _size;
        private FontStyle _style;

        public FontFamily Family
        {
            get => _family;
            set
            {
                _family = value;
                NotifyOfPropertyChange();
            }
        }

        public FontWeight Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                NotifyOfPropertyChange();
            }
        }

        public FontStretch Stretch
        {
            get => _stretch;
            set
            {
                _stretch = value;
                NotifyOfPropertyChange();
            }
        }

        public double Size
        {
            get => _size;
            set
            {
                _size = value;
                NotifyOfPropertyChange();
            }
        }

        public FontStyle Style
        {
            get => _style;
            set
            {
                _style = value;
                NotifyOfPropertyChange();
            }
        }

        public FontViewModel(IThemeProvider themeProvider)
        {
            _family = themeProvider.ButtonFont.Family;
            _size = themeProvider.ButtonFont.Size;
            _stretch = themeProvider.ButtonFont.Stretch;
            _style = themeProvider.ButtonFont.Style;
            _weight = themeProvider.ButtonFont.Weight;
        }
    }
}