using Eto.Drawing;
using Eto.Forms;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyViewModel : BaseViewModel
    {
        private string _name;
        private Color _color;
        private Color _highlightedColor;
        private bool _isHighlighted;

        private static readonly Color DefaultColor = (new Button()).BackgroundColor;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public Color HighlightedColor
        {
            get => _highlightedColor;
            set
            {
                SetProperty(ref _highlightedColor, value);
                UpdateColor();
            }
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                SetProperty(ref _isHighlighted, value);
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            Color = IsHighlighted ? HighlightedColor : DefaultColor;
        }
    }
}