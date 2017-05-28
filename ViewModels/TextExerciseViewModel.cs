using Eto.Forms;
using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace NTouchTypeTrainer.ViewModels
{
    public class TextExerciseViewModel : BaseViewModel
    {
        private string _text;
        private int _textHighlightIndex;
        private int _textHighlightLength;
        private int _mappingTargetSequenceIndex;
        private ObservableCollection<IMappingTarget> _mappingTargetSequence;
        private ObservableCollection<IMappingTarget> _pressedMappingTargetSequence;

        private ICollection<Range<int>> _hardwareKeyTextIndexes;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public int TextHighlightIndex
        {
            get => _textHighlightIndex;
            set => SetProperty(ref _textHighlightIndex, value);
        }

        public int TextHighlightLength
        {
            get => _textHighlightLength;
            set => SetProperty(ref _textHighlightLength, value);
        }

        public ObservableCollection<IMappingTarget> MappingTargetSequence
        {
            get => _mappingTargetSequence;
            set
            {
                _mappingTargetSequence = value;
                OnPropertyChanged();
            }
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public int MappingTargetSequenceIndex
        {
            get => _mappingTargetSequenceIndex;
            set
            {
                _mappingTargetSequenceIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentMappingTarget));
            }
        }

        public IMappingTarget CurrentMappingTarget
            => (MappingTargetSequenceIndex >= 0 && MappingTargetSequenceIndex < MappingTargetSequence.Count)
                ? MappingTargetSequence[MappingTargetSequenceIndex]
                : null;

        /// <summary>
        /// Holds a list of start- and stop indexes for the control sequences in the Text. 
        /// E.g. "Press Ctrl+C to copy something": starting index is 6, end index is 12. 
        /// "Ctrl+C" will get highlighted differently to indicate that it is a key / key combination and not regular text. 
        /// </summary>
        public ICollection<Range<int>> HardwareKeyTextIndexes
        {
            get => _hardwareKeyTextIndexes;
            set
            {
                _hardwareKeyTextIndexes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Keys pressed by the user. Used to compare the users input against the MappingTargetSequence to train.
        /// </summary>
        public ObservableCollection<IMappingTarget> PressedTargetSequence
        {
            get => _pressedMappingTargetSequence;
            private set
            {
                _pressedMappingTargetSequence = value;
                OnPropertyChanged();
            }
        }

        public TextExerciseViewModel()
        {
            _mappingTargetSequence = new ObservableCollection<IMappingTarget>();
            _hardwareKeyTextIndexes = new List<Range<int>>();
            _pressedMappingTargetSequence = new ObservableCollection<IMappingTarget>();

            InitIndexes();
        }

        public void SetExercise(IExercise exercise)
        {
            TextHighlightIndex = 0;
            MappingTargetSequence = new ObservableCollection<IMappingTarget>(exercise.Sequence);

            InitIndexes();
            UpdateTextAndIndexesBySequence();
        }

        public void InitIndexes()
        {
            MappingTargetSequenceIndex = 0;
            TextHighlightIndex = 0;
            TextHighlightLength = GetLength(CurrentMappingTarget);
        }

        public void EvaluateInput(MappedHardwareKey pressedKey)
        {
            var expected = CurrentMappingTarget;
            if (pressedKey != null && pressedKey.Equals(expected))
            {
                PressedTargetSequence.Add(pressedKey);
                SetIndexesToNextKey();
            }
            else
            {
                var isBackspace = (pressedKey != null && pressedKey.HardwareKey == HardwareKey.Backspace && pressedKey.Modifiers == Modifier.None);
                var isOtherKeyPressedPreviously = PressedTargetSequence.Count > 0;
                var backspaceToDeleteLastEntry = isBackspace && isOtherKeyPressedPreviously;
                if (backspaceToDeleteLastEntry)
                {
                    PressedTargetSequence.RemoveAt(PressedTargetSequence.Count - 1);
                    SetIndexToPreviousKey();
                }
                else
                {
                    // ToDo: Add error highlighting etc
                    PressedTargetSequence.Add(pressedKey ?? new MappedHardwareKey(HardwareKey.AccentGrave));    // ToDo: Handle null better...
                    SetIndexesToNextKey();
                }
            }
        }

        private void SetIndexesToNextKey()
        {
            if (CurrentMappingTarget != null)
            {
                var currentTextLength = GetLength(CurrentMappingTarget);

                MappingTargetSequenceIndex++;

                TextHighlightIndex += currentTextLength;
                TextHighlightLength = GetLength(CurrentMappingTarget);
            }
            else
            {
                SetIndexToInvalidKey();
            }
        }

        private void SetIndexToPreviousKey()
        {
            if (CurrentMappingTarget != null)
            {
                var currentTextLength = GetLength(CurrentMappingTarget);

                MappingTargetSequenceIndex--;

                var oldTextLength = GetLength(CurrentMappingTarget);

                TextHighlightIndex -= currentTextLength + oldTextLength;
                TextHighlightLength = GetLength(CurrentMappingTarget);
            }
            else
            {
                SetIndexToInvalidKey();
            }
        }

        private void SetIndexToInvalidKey()
        {
            MappingTargetSequenceIndex = -1;
            TextHighlightIndex = -1;
            TextHighlightLength = 0;
        }

        private int GetLength(IMappingTarget key)
            => GetLengthAndRepresentation(key).RtfTextLength;

        private (int RtfTextLength, string StringRepresentation, bool IsKeyCombination) GetLengthAndRepresentation(IMappingTarget key)
        {
            var keyInfos = (rtfLength: 0, stringRepresentation: string.Empty, isKeyCombination: false);

            if (key is MappedCharacter mappedChar)
            {
                keyInfos = (rtfLength: 1, stringRepresentation: mappedChar.Character.ToString(), isKeyCombination: false);
            }
            else if (key is MappedHardwareKey hardwareKey)
            {
                keyInfos = (rtfLength: hardwareKey.Name.Length, stringRepresentation: hardwareKey.Name, isKeyCombination: true);

                var isNewLine = hardwareKey.HardwareKey == HardwareKey.Enter && hardwareKey.Modifiers == Modifier.None;
                if (isNewLine)
                {
                    keyInfos.stringRepresentation += Environment.NewLine;
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

            return keyInfos;
        }

        private void UpdateTextAndIndexesBySequence()
        {
            _hardwareKeyTextIndexes.Clear();

            var textBuilder = new StringBuilder();
            var iRtfText = 0;

            MappingTargetSequence
                .Where(mappingTarget => mappingTarget != null)
                .ForEach(mappingTarget =>
                {
                    var (rtfTextLength, stringRepresentation, isKeyCombination) = GetLengthAndRepresentation(mappingTarget);

                    textBuilder.Append(stringRepresentation);

                    if (isKeyCombination)
                    {
                        HardwareKeyTextIndexes.Add(
                            new Range<int>(iRtfText, iRtfText + rtfTextLength - 1));
                    }

                    iRtfText += rtfTextLength;
                });

            Text = textBuilder.ToString();
        }
    }
}