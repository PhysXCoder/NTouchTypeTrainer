using Caliburn.Micro;
using Eto.Forms;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Interfaces.Common.Graphics;
using NTouchTypeTrainer.Interfaces.Views;
using NTouchTypeTrainer.ViewModels;
using NTouchTypeTrainer.Views.Common;
using NTouchTypeTrainer.Views.Controls;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NTouchTypeTrainer.Views
{
    public class KeyboardView : Panel
    {
        private readonly ISharedSizeGroup _regularKeySharedSizeGroup;

        private StackLayout _verticalStackLayout;

        private readonly IEventAggregator _eventAggregator;
        private readonly IGraphicsProvider _graphicsProvider;

        public KeyboardView(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _eventAggregator = eventAggregator;
            _graphicsProvider = graphicsProvider;

            _regularKeySharedSizeGroup = new SharedSizeGroup(eventAggregator, "RegularKeys");

            DataContextChanged += KeyboardView_DataContextChanged;

            InitRows();

            Content = _verticalStackLayout;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void KeyboardView_DataContextChanged(object sender, EventArgs e)
        {
            InitRows();
        }

        private void InitRows()
        {
            if (_verticalStackLayout == null)
            {
                _verticalStackLayout = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    Padding = 10,
                    Spacing = 5,
                };
            }

            FillVerticalStackLayout();
        }

        private void FillVerticalStackLayout()
        {
            _verticalStackLayout.Items.Clear();

            if (DataContext is KeyboardViewModel keyboardViewModel)
            {
                var mechLayoutVm = keyboardViewModel.MechanicalKeyboardLayoutViewModel;

                foreach (var iRow in mechLayoutVm.KeyIndexsInRow.Keys)
                {
                    var row = new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Padding = 0,
                        Spacing = 5,
                    };

                    var keyIndexes = mechLayoutVm.KeyIndexsInRow[iRow];
                    for (var iKey = keyIndexes.Min(); iKey <= keyIndexes.Max(); iKey++)
                    {
                        var keyPos = new KeyPosition(iRow, iKey);
                        var keySize = mechLayoutVm.KeySizes[keyPos];

                        var keyView = (keySize == null)
                            ? (HardwareKeyControl)CreateRegularSharedSizeKeyControl(keyPos)
                            : CreateProportionalSizeKeyControl(keyPos, keySize.Value);

                        keyView.DataContext = keyboardViewModel.AllKeysViewModel.KeysByPosition[keyPos];

                        row.Items.Add(keyView);
                    }

                    _verticalStackLayout.Items.Add(row);
                }
            }
        }

        private SharedSizeHardwareKeyControl CreateRegularSharedSizeKeyControl(KeyPosition keyPosition)
        {
            var keyControl = new SharedSizeHardwareKeyControl(keyPosition, _regularKeySharedSizeGroup, _eventAggregator, _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }

        private ProportionalSizeHardwareKeyControl CreateProportionalSizeKeyControl(KeyPosition keyPosition, float widthFactor)
        {
            var keyControl = new ProportionalSizeHardwareKeyControl(
                keyPosition,
                widthFactor,
                _regularKeySharedSizeGroup,
                _eventAggregator,
                _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }
    }
}