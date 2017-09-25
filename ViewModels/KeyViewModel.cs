using Caliburn.Micro;
using NTouchTypeTrainer.Common.Drawing;
using NTouchTypeTrainer.Common.Gui;
using NTouchTypeTrainer.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.Messages;
using System.Collections.Generic;
using System.Windows.Media;
using Size = System.Windows.Size;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyViewModel : BaseViewModel, IHandle<SizeGroupChangedMsg>
    {
        private readonly IThemeProvider _themeProvider;

        private string _name;
        private KeyPosition _keyPosition;
        private ISizeGroup _sizeGroup;
        private Brush _normalBackgroundBrush;
        private Brush _highlightedBackgroundBrush;
        private Brush _normalForegroundBrush;
        private Brush _highlightedForegroundBrush;
        private bool _isHighlighted;
        private Size _size;
        private FontViewModel _font;
        private IDictionary<IKeyboardKey, IMappingTarget> _mappings;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                UpdateSizeRequest();
            }
        }

        public KeyPosition KeyPosition
        {
            get => _keyPosition;
            set
            {
                _keyPosition = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                SetProperty(ref _isHighlighted, value);
                NotifyOfPropertyChange();
                UpdateColors();
            }
        }

        public Brush NormalBackgroundBrush
        {
            private get => _normalBackgroundBrush;
            set
            {
                _normalBackgroundBrush = value;
                NotifyOfPropertyChange();
                UpdateColors();
            }
        }

        public Brush HighlightedBackgroundBrush
        {
            private get => _highlightedBackgroundBrush;
            set
            {
                _highlightedBackgroundBrush = value;
                NotifyOfPropertyChange();
                UpdateColors();
            }
        }

        public Brush CurrentBackgroundBrush =>
            IsHighlighted ? HighlightedBackgroundBrush : NormalBackgroundBrush;

        public Brush NormalForegroundBrush
        {
            private get => _normalForegroundBrush;
            set
            {
                _normalForegroundBrush = value;
                NotifyOfPropertyChange();
                UpdateColors();
            }
        }

        public Brush HighlightedForegroundBrush
        {
            private get => _highlightedForegroundBrush;
            set
            {
                _highlightedForegroundBrush = value;
                NotifyOfPropertyChange();
                UpdateColors();
            }
        }

        // ToDo: Foreground-Color in FingerColors file (t3c)?
        public Brush CurrentForegroundBrush =>
            IsHighlighted ? HighlightedForegroundBrush : NormalForegroundBrush;

        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                NotifyOfPropertyChange();
            }
        }

        public FontViewModel Font
        {
            get => _font;
            set
            {
                _font = value;
                NotifyOfPropertyChange();
            }
        }

        public ISizeGroup SizeGroup
        {
            get => _sizeGroup;
            set
            {
                SizeGroup?.ClearRequests(this);
                _sizeGroup = value;
                NotifyOfPropertyChange();
            }
        }

        public IDictionary<IKeyboardKey, IMappingTarget> Mappings
        {
            get => _mappings;
            set
            {
                _mappings = value;
                NotifyOfPropertyChange();
            }
        }

        public KeyViewModel(IThemeProvider themeProvider, ISizeGroup sizeGroup, IEventAggregator eventAggregator)
        {
            _keyPosition = new KeyPosition(-1, -1);
            _themeProvider = themeProvider;
            _font = new FontViewModel(themeProvider);

            SizeGroup = sizeGroup;

            InitParameters();
            eventAggregator.Subscribe(this);
        }

        private void InitParameters()
        {
            _isHighlighted = false;
            _name = "DefaultKeyName";
            _size = new Size(1, 1);

            _normalBackgroundBrush = _themeProvider.ButtonBackgroundBrush;
            _highlightedBackgroundBrush = _themeProvider.ButtonBackgroundBrush;
            _normalForegroundBrush = _themeProvider.ButtonForegroundBrush;
            _highlightedForegroundBrush = _themeProvider.ButtonForegroundBrush;

            UpdateColors();
            UpdateSizeRequest();
        }

        private void UpdateColors()
        {
            NotifyOfPropertyChange(nameof(CurrentBackgroundBrush));
            NotifyOfPropertyChange(nameof(CurrentForegroundBrush));
        }

        private void UpdateSizeRequest()
        {
            var nameSize = StringMeasurer.MeasureString(
                Name, Font);

            nameSize = nameSize.Increase(_themeProvider.PaddingSize.Multiply(2));

            SizeGroup.RequestMinSize(this, nameSize);
        }

        void IHandle<SizeGroupChangedMsg>.Handle(SizeGroupChangedMsg message)
        {
            if (message != null && message.Sender == SizeGroup)
            {
                Size = message.Sender.Size;
            }
        }
    }
}