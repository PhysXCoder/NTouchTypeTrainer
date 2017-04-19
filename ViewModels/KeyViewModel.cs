using System;
using Eto.Drawing;
using Eto.Forms;

namespace NTouchTypeTrainer.ViewModels
{
#pragma warning disable CS0659
    public class KeyViewModel : BaseViewModel, IEquatable<KeyViewModel>
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

        public bool Equals(KeyViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name) && _highlightedColor.Equals(other._highlightedColor) && _isHighlighted == other._isHighlighted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyViewModel)obj);
        }

        private void UpdateColor()
        {
            Color = IsHighlighted ? HighlightedColor : DefaultColor;
        }
    }
#pragma warning restore CS0659
}