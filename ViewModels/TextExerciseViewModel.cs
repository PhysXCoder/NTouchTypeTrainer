using Caliburn.Micro;
using NTouchTypeTrainer.Common.DataStructures;
using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using NTouchTypeTrainer.Interfaces.Common.Sound;
using NTouchTypeTrainer.Interfaces.Domain.Exercises;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace NTouchTypeTrainer.ViewModels
{
    public class TextExerciseViewModel : BaseViewModel, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IThemeProvider _themeProvider;
        private readonly ISoundPlayer _soundPlayer;

        private IRunningExercise _runningExercise;
        private IMappingTarget _lastCurrentMappingTarget;
        private string _text;
        private FlowDocument _flowDocument;
        private Range<int> _selectedTextRange;
        private int _mappingTargetSequenceIndex;
        private ObservableCollection<IMappingTarget> _mappingTargetSequence;

        // ToDo: Several, configurable sounds
        private static readonly Uri ErrorSound =
            // new Uri("pack://application:,,,/Resources/Sounds/142608__autistic-lucario__error.wav");            
            // new Uri("pack://application:,,,/Resources/Sounds/363920__samsterbirdies__8-bit-error.wav");
            new Uri("pack://application:,,,/Resources/Sounds/325113__fisch12345__error.wav");

        public string Text
        {
            get => _text;
            private set => SetProperty(ref _text, value);
        }

        public FlowDocument FlowDocument
        {
            get => _flowDocument;
            private set
            {
                _flowDocument = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<IMappingTarget> MappingTargetSequence
        {
            get => _mappingTargetSequence;
            private set
            {
                _mappingTargetSequence = value;
                NotifyOfPropertyChange();
                CheckCurrentMappingTargetChange();
            }
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public int MappingTargetSequenceIndex
        {
            get => _mappingTargetSequenceIndex;
            private set
            {
                _mappingTargetSequenceIndex = value;
                NotifyOfPropertyChange();
                CheckCurrentMappingTargetChange();
            }
        }

        private void CheckCurrentMappingTargetChange()
        {
            var changed = false;
            if (CurrentMappingTarget == null || _lastCurrentMappingTarget == null)
            {
                changed |= (CurrentMappingTarget != null || _lastCurrentMappingTarget != null);
            }
            else
            {
                changed = !CurrentMappingTarget.Equals(_lastCurrentMappingTarget);
            }

            _lastCurrentMappingTarget = CurrentMappingTarget;
            if (changed)
            {
                NotifyOfPropertyChange(nameof(CurrentMappingTarget));

                _eventAggregator.PublishOnCurrentThread(new ExpectedKeyChangedMsg()
                {
                    Sender = this,
                    NewExpectedMappingTarget = CurrentMappingTarget
                });
            }

        }

        public IMappingTarget CurrentMappingTarget
            => (MappingTargetSequenceIndex >= 0 && MappingTargetSequenceIndex < MappingTargetSequence.Count)
                ? MappingTargetSequence[MappingTargetSequenceIndex]
                : null;

        /// <summary>
        /// Holds a list of start and stop indexes for the control sequences in the Text. 
        /// E.g. "Press Ctrl+C to copy something": starting index is 6, end index is 12. 
        /// "Ctrl+C" will get highlighted differently to indicate that it is a key / key combination and not regular text. 
        /// </summary>
        public ObservableCollection<Range<int>> HardwareKeyTextRanges { get; }

        /// <summary>
        /// Holds a list of start and stop indexes for wrongly pressed keys / key combinations.
        /// </summary>
        public ObservableCollection<Range<int>> WrongTargetsRanges { get; }

        /// <summary>
        /// Keys pressed by the user. Used to compare the users input against the MappingTargetSequence to train.
        /// </summary>
        public ObservableCollection<IMappingTarget> PressedTargetSequence { get; }

        public TextExerciseViewModel(
            IThemeProvider themeProvider,
            IEventAggregator eventAggregator,
            ISoundPlayer soundPlayer)
        {
            _themeProvider = themeProvider;
            _eventAggregator = eventAggregator;
            _soundPlayer = soundPlayer;

            _flowDocument = new FlowDocument(new Paragraph());
            _mappingTargetSequence = new ObservableCollection<IMappingTarget>();
            HardwareKeyTextRanges = new ObservableCollection<Range<int>>();
            WrongTargetsRanges = new ObservableCollection<Range<int>>();
            PressedTargetSequence = new ObservableCollection<IMappingTarget>();

            InitIndexes();
        }

        public KeyCorrectness EvaluateInput(IHardwareKeyMappingTarget pressedKey)
        {
            var correctness = _runningExercise?.EvaluateKeyUp(pressedKey) ?? KeyCorrectness.Indeterminate;

            if (correctness == KeyCorrectness.Wrong)
            {
                _soundPlayer.Play(ErrorSound);
            }

            return correctness;
        }

        public void SetRunningExercise(IRunningExercise runningExercise)
        {
            if (_runningExercise != null)
            {
                _runningExercise.PressedSequenceChanged -= _runningExercise_PressedSequenceChanged;
            }
            _runningExercise = runningExercise;
            _runningExercise.PressedSequenceChanged += _runningExercise_PressedSequenceChanged;

            MappingTargetSequence = new ObservableCollection<IMappingTarget>(runningExercise.Exercise.ExpectedSequence);

            InitIndexes();
            UpdateNonselectionHighlightingIndexesBySequence();
            UpdateFlowDocument();
        }

        private void _runningExercise_PressedSequenceChanged(IRunningExercise obj)
        {
            // ToDo: Add error highlighting etc
            UpdateMappingTargetSequenceIndex();
            UpdateSelectionandErrorRanges();
            UpdateFlowDocument();
        }

        private void InitIndexes()
        {
            MappingTargetSequenceIndex = 0;
            _selectedTextRange = new Range<int>(0, GetLength(CurrentMappingTarget) - 1);
        }

        private void SetIndexToInvalidKey()
        {
            MappingTargetSequenceIndex = -1;
            _selectedTextRange = new Range<int>(-1, -1);
        }

        private int GetLength(IMappingTarget key)
            => GetLengthAndRepresentation(key).TextLength;

        private (int TextLength, string StringRepresentation, bool IsKeyCombination)
        GetLengthAndRepresentation(IMappingTarget key)
        {
            var keyInfos = (length: 0, stringRepresentation: String.Empty, isKeyCombination: false);

            // Check for hardware key. Get length.
            if (key is CharacterMappingTarget mappedChar)
            {
                keyInfos = (length: 1, stringRepresentation: mappedChar.Character.ToString(), isKeyCombination: false);
            }
            else if (key is HardwareKeyMappingTarget hardwareKey)
            {
                keyInfos = (length: hardwareKey.Name.Length, stringRepresentation: hardwareKey.Name, isKeyCombination: true);

                var isNewLine = hardwareKey.HardwareKey == HardwareKey.Enter && hardwareKey.Modifiers == Modifier.None;
                if (isNewLine)
                {
                    keyInfos.stringRepresentation += Environment.NewLine;
                    keyInfos.length += Environment.NewLine.Length;
                }
            }
            else if (key == null)
            {
                // return default values from above
            }
            else
            {
                throw new NotImplementedException(
                    $"{nameof(IMappingTarget)}'s runtime-type '{key.GetType().Name}' is not handled in "
                    + $"{nameof(TextExerciseViewModel)}.{nameof(GetLengthAndRepresentation)}!");
            }

            // Return infos
            return keyInfos;
        }

        private void UpdateNonselectionHighlightingIndexesBySequence()
        {
            HardwareKeyTextRanges.Clear();

            var textBuilder = new StringBuilder();
            var iRtfText = 0;

            MappingTargetSequence
                .Where(mappingTarget => mappingTarget != null)
                .ForEach(mappingTarget =>
                {
                    var (rtfTextLength, stringRepresentation, isKeyCombination) = GetLengthAndRepresentation(mappingTarget);

                    textBuilder.Append(stringRepresentation);

                    var currentRange = new Range<int>(iRtfText, iRtfText + rtfTextLength - 1);

                    if (isKeyCombination)
                    {
                        HardwareKeyTextRanges.Add(currentRange);
                    }


                    iRtfText += rtfTextLength;
                });

            Text = textBuilder.ToString();
        }

        private void UpdateSelectionandErrorRanges()
        {
            var textLength = 0;
            WrongTargetsRanges.Clear();

            var expectedMappingTargets = _runningExercise.Exercise.ExpectedSequence.ToList();
            for (var iTarget = 0; iTarget < expectedMappingTargets.Count; iTarget++)
            {
                var currentMappingTarget = expectedMappingTargets[iTarget];
                var currentLength = GetLength(currentMappingTarget);
                var currentRange = new Range<int>(textLength, textLength + currentLength - 1);

                if (iTarget < _runningExercise.PressedSequence.Count)
                {
                    var isWrong = _runningExercise.ErrorIndexes.Contains(iTarget);
                    if (isWrong)
                    {
                        WrongTargetsRanges.Add(currentRange);
                    }
                }

                var iterationEnd = (_runningExercise.NextExpectedIndex == iTarget);
                if (!iterationEnd)
                {
                    textLength += currentLength;
                }
                else
                {
                    _selectedTextRange = currentRange;
                    return;
                }
            }

            // If code execution is here, exercise has ended / sufficient keys have been pressed
            SetIndexToInvalidKey();
        }

        private void UpdateMappingTargetSequenceIndex()
        {
            MappingTargetSequenceIndex = _runningExercise.PressedSequence.Count;
        }

        private void UpdateFlowDocument()
        {
            // Prepare the ducoment
            var paragraph = new Paragraph();
            FlowDocument = new FlowDocument(paragraph)
            {
                FontFamily = _themeProvider.TextFontFamily
            };
            FlowDocument.FontSize += 3;

            // Get ranges for marking / highlighting
            var highlightedKeyRanges = GetKeyRanges();
            var orderedRangesStack = new Stack<Range<int>>(
                highlightedKeyRanges.OrderByDescending(range => range.Start));

            // Build the flow document considering the ranges from above
            var currentTextIndex = 0;
            while (currentTextIndex < Text.Length)
            {
                var highlightedTextRange = GetNextTextRange(orderedRangesStack, currentTextIndex);

                // First copy the unformatted text (up to the range)
                var run = CreateRegularTextRun(currentTextIndex, highlightedTextRange);
                if (run != null)
                {
                    paragraph.Inlines.Add(run);
                }

                // Then the highlighted / marked text
                run = CreateHighlightedTextRun(highlightedTextRange);
                if (run != null)
                {
                    paragraph.Inlines.Add(run);
                }

                // Advance index
                currentTextIndex = highlightedTextRange?.Stop + 1 ?? Text.Length;
            }
        }

        private IEnumerable<Range<int>> GetKeyRanges()
        {
            // 1. Hardware key ranges
            IList<Range<int>> keyRanges = HardwareKeyTextRanges.ToList();

            // 2. Selection highlighting range
            var isValidSelectionHighlighting =
                _selectedTextRange.Start >= 0 &&
                _selectedTextRange.Stop < Text.Length;

            if (isValidSelectionHighlighting)
            {
                // Avoid collisions (collision = there's a hardware key range at the same index)
                // Highlighting range has higher priority. So filter out the hardware key ranges when conflicting.
                // Then add highlighting range.
                keyRanges = HardwareKeyTextRanges
                    .Where(range => range.Start != _selectedTextRange.Start)
                    .Concat(new[] { _selectedTextRange })
                    .ToList();
            }

            // 3. Error highlighting             
            keyRanges = keyRanges
                .Concat(WrongTargetsRanges.Where(range => keyRanges.All(r => r.Start != range.Start)))  // Again avoid collissions
                .ToList();

            return keyRanges;
        }

        private static Range<int> GetNextTextRange(Stack<Range<int>> orderedRanges, int currentIndex)
        {
            Range<int> range = null;
            var isRangeValid = false;

            // Advance 1 position (or as many as necessary) so that we have the next range of highlighted / marked characters
            while (!isRangeValid && orderedRanges.Any())
            {
                range = orderedRanges.Pop();
                isRangeValid = currentIndex <= range.Start;
            }

            // None found? Then return null
            return isRangeValid ? range : null;
        }

        private Run CreateRegularTextRun(int currentIndex, Range<int> currentRange)
        {
            var iTextStart = currentIndex;
            var iTextStop = currentRange?.Start ?? Text.Length;
            var textLen = iTextStop - iTextStart;
            Run run = null;
            if (textLen > 0)
            {
                var currentText = Text.Substring(iTextStart, textLen);
                run = new Run(currentText);
                ColorTextElementIfWrong(run, new Range<int>(iTextStart, iTextStop));
            }
            return run;
        }

        private void ColorTextElementIfWrong(TextElement run, Range<int> currentRange)
        {
            if (IsWrong(currentRange))
            {
                run.Foreground = _themeProvider.WrongTextBrush;
                run.Background = _themeProvider.WrongTextBackgroundBrush;
            }
        }

        private bool IsWrong(Range<int> rangeToCheck)
        {
            return WrongTargetsRanges.Any(range => range != null && range.Equals(rangeToCheck));
        }

        private Run CreateHighlightedTextRun(Range<int> currentRange)
        {
            if (currentRange == null)
            {
                return null;
            }

            var currentText = Text.Substring(currentRange.Start, currentRange.Stop - currentRange.Start + 1);
            var run = new Run(currentText);

            var isSelectionHighlighting = currentRange.Equals(_selectedTextRange);
            var isWrong = IsWrong(currentRange);
            if (isSelectionHighlighting)
            {
                // selection hightlighting
                run.Background = new SolidColorBrush(Colors.Blue);  // ToDo: get these via theme provider
                run.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (!isWrong)
            {
                // hardwarekey highlighting
                run.Background = new SolidColorBrush(Colors.Gray);
            }

            ColorTextElementIfWrong(run, currentRange);

            return run;
        }

        public void Dispose()
        {
            if (_runningExercise != null)
            {
                _runningExercise.PressedSequenceChanged -= _runningExercise_PressedSequenceChanged;
            }
        }
    }
}