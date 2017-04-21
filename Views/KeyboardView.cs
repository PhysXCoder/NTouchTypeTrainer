using System;
using System.Diagnostics.CodeAnalysis;
using Caliburn.Micro;
using Eto.Forms;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.ViewModels;
using NTouchTypeTrainer.Views.Common;
using NTouchTypeTrainer.Views.Controls;

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

            var keyboardViewModel = DataContext as KeyboardViewModel;
            if (keyboardViewModel != null)
            {
                foreach (var keyRow in keyboardViewModel.MechanicalKeyboardLayoutViewModel.Rows)
                {
                    var row = new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Padding = 0,
                        Spacing = 5,
                    };

                    foreach (var hardwareKey in keyRow)
                    {
                        var keyView = CreateRegularSharedSizeKeyControl(hardwareKey);   // ToDo: Also add ProportionalSized controls
                        keyView.DataContext = keyboardViewModel.AllKeysViewModel.Keys[hardwareKey];

                        row.Items.Add(keyView);
                    }

                    _verticalStackLayout.Items.Add(row);
                }
            }
        }

        private SharedSizeHardwareKeyControl CreateRegularSharedSizeKeyControl(HardwareKey key)
        {
            var keyControl = new SharedSizeHardwareKeyControl(key, _regularKeySharedSizeGroup, _eventAggregator, _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }

        private ProportionalSizeHardwareKeyControl CreateProportionalSizeKeyControl(HardwareKey key, float widthFactor)
        {
            var keyControl = new ProportionalSizeHardwareKeyControl(
                key,
                widthFactor,
                _regularKeySharedSizeGroup,
                _eventAggregator,
                _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }
    }
}