using System;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.ViewModels;
using System.ComponentModel;

namespace NTouchTypeTrainer.Views
{
    public class TextExerciseView : Panel
    {
        private readonly Color _textColor = SystemColors.ControlText;
        private readonly Color _backgroundColor = SystemColors.ControlBackground;
        private readonly Color _caretTextColor = SystemColors.HighlightText;
        private readonly Color _caretBackgroundColor = SystemColors.Highlight;
        private readonly Color _keyCombinationTextColor = SystemColors.ControlBackground;
        private readonly Color _keyCombinationBackgroundColor = SystemColors.DisabledText;

        private readonly RichTextArea _rtfArea;
        private TextExerciseViewModel _oldDataContext;

        private readonly Button _btnTest;
        private readonly StackLayout _autoHighlightViewerStackLayout;
        private readonly TextBox _startIndexTextBox;
        private readonly TextBox _stopIndexTextBox;
        private readonly StackLayout _stackLayout;
        private readonly StackLayout _manualHighlightSetterStackLayout;
        private readonly NumericUpDown _startUpDown;
        private readonly NumericUpDown _endUpDown;

        public TextExerciseView()
        {
            _rtfArea = new RichTextArea
            {
                Text = "",
                ReadOnly = true,

                Width = 800,
                Height = 200,
            };

            _btnTest = new Button
            {
                Text = "Test"
            };
            _startIndexTextBox = new TextBox
            {
                ReadOnly = true
            };
            _stopIndexTextBox = new TextBox
            {
                ReadOnly = true
            };
            _autoHighlightViewerStackLayout = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items = { new Label { Text = "Hightlight start idx" }, _startIndexTextBox, new Label { Text = "Hightlight stop idx" }, _stopIndexTextBox }
            };
            _startUpDown = new NumericUpDown
            {
                Width = 100
            };
            _endUpDown = new NumericUpDown
            {
                Width = 100
            };
            _manualHighlightSetterStackLayout = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items = { new Label { Text = "Start idx" }, _startUpDown, new Label { Text = "Stop idx" }, _endUpDown }
            };
            _stackLayout = new StackLayout
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Items = { _rtfArea, _btnTest, _autoHighlightViewerStackLayout, _manualHighlightSetterStackLayout },
            };

            //Content = _rtfArea;
            Content = _stackLayout;

            //SizeChanged += (sender, e) => { _rtfArea.Size = Size; };
            SizeChanged += (sender, e) => { _stackLayout.Size = Size; };
            DataContextChanged += (sender, e) => ViewDataContext();

            _btnTest.Click += (sender, e) => { (DataContext as TextExerciseViewModel)?.EvaluateInput(null); };
            _startUpDown.ValueChanged += StartUpDownOnValueChanged;
            _endUpDown.ValueChanged += StartUpDownOnValueChanged;
        }

        private void StartUpDownOnValueChanged(object o, EventArgs eventArgs)
        {
            var startIndex = (int)_startUpDown.Value;
            var stopIndex = (int)_endUpDown.Value;

            RefreshKeyCombinationIndexesColors();
            var nextKeyRange = new Range<int>(startIndex, stopIndex);
            _rtfArea.Buffer.SetForeground(nextKeyRange, _caretTextColor);
            _rtfArea.Buffer.SetBackground(nextKeyRange, _caretBackgroundColor);
        }

        private void ViewDataContext()
        {
            if (_oldDataContext != null)
            {
                _oldDataContext.PropertyChanged -= TextExerciseViewModelOnPropertyChanged;
            }

            if (DataContext is TextExerciseViewModel textExerciseViewModel)
            {
                _oldDataContext = textExerciseViewModel;

                // Can't use default Eto.Forms binding because of special bindings for RtfTextArea.
                // Therefor, manually subscribe to the PropertyChanged event.
                textExerciseViewModel.PropertyChanged += TextExerciseViewModelOnPropertyChanged;

                // Execute each binding at least once:
                UpdateText();
                RefreshKeyCombinationIndexesColors();
                UpdateHightlightedText();
            }
        }

        private void TextExerciseViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(TextExerciseViewModel.Text):
                    UpdateText();
                    break;

                case nameof(TextExerciseViewModel.HardwareKeyTextIndexes):
                    RefreshKeyCombinationIndexesColors();
                    UpdateHightlightedText();
                    break;

                case nameof(TextExerciseViewModel.TextHighlightIndex):
                case nameof(TextExerciseViewModel.TextHighlightLength):
                    UpdateHightlightedText();
                    break;

                default:
                    break;
            }
        }

        private void UpdateText()
        {
            _rtfArea.Text = ((TextExerciseViewModel)DataContext).Text;
        }

        private void UpdateHightlightedText()
        {
            var vm = ((TextExerciseViewModel)DataContext);
            var newHightlightedIndex = vm.TextHighlightIndex;
            var newHightlightIndexStop = newHightlightedIndex + vm.TextHighlightLength - 1;

            _rtfArea.CaretIndex = newHightlightedIndex;    // This seems to have no effect on the rich text box :-(

            // Workaround to highlight the next character, since assigning CaretIndex doesn't work:
            RefreshKeyCombinationIndexesColors();
            var nextKeyRange = new Range<int>(newHightlightedIndex, newHightlightIndexStop);
            _rtfArea.Buffer.SetForeground(nextKeyRange, _caretTextColor);
            _rtfArea.Buffer.SetBackground(nextKeyRange, _caretBackgroundColor);

            // ToDo: Remove
            _startIndexTextBox.Text = newHightlightedIndex.ToString();
            _stopIndexTextBox.Text = newHightlightIndexStop.ToString();
        }

        private void RefreshKeyCombinationIndexesColors()
        {
            ClearKeyCombinationColors();
            SetKeyCombinationColors();
        }

        private void ClearKeyCombinationColors()
        {
            var wholeTextRange = new Range<int>(0, _rtfArea.Text.Length - 1);

            _rtfArea.Buffer.SetBackground(wholeTextRange, _backgroundColor);
            _rtfArea.Buffer.SetForeground(wholeTextRange, _textColor);
        }

        private void SetKeyCombinationColors()
        {
            (DataContext as TextExerciseViewModel)?.HardwareKeyTextIndexes.ForEach(range =>
            {
                _rtfArea.Buffer.SetForeground(range, _keyCombinationTextColor);
                _rtfArea.Buffer.SetBackground(range, _keyCombinationBackgroundColor);
            });
        }
    }
}