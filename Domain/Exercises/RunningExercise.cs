using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain.Exercises;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public class RunningExercise : IRunningExercise
    {
        private readonly List<IMappingTarget> _pressedSequence;
        private readonly List<int> _errorIndexes;

        public IReadOnlyList<IMappingTarget> PressedSequence
            => _pressedSequence.AsReadOnly();

        public IReadOnlyList<int> ErrorIndexes
            => _errorIndexes.AsReadOnly();

        public int NextExpectedIndex
            => _pressedSequence.Count;

        // ToDo: Extract this into options (as well as colors and the error sound and prhaps more)
        public bool AllowCorrections { get; set; }

        /// <summary>
        /// Null OR reference to the next expected mapping target
        /// </summary>
        public IMappingTarget NextExpectedMappingTarget
        {
            get
            {
                var expected = Exercise.ExpectedSequence.ToList();
                var index = NextExpectedIndex;
                if (0 <= index && index < expected.Count)
                {
                    return expected[index]; // wehn 3 keys have been pressed, next expected is the 4th key. Index of the 4th key is 3. Therfore just take Count of the pressed sequence
                }

                return null;
            }
        }

        IReadOnlyList<IMappingTarget> IRunningExercise.PressedSequence
            => PressedSequence;

        public IExercise Exercise { get; }

        public event Action<IRunningExercise> PressedSequenceChanged;

        public RunningExercise(IExercise exercise)
        {
            _pressedSequence = new List<IMappingTarget>();
            _errorIndexes = new List<int>();
            Exercise = exercise;
        }

        public KeyCorrectness EvaluateKeyUp(IHardwareKeyMappingTarget pressedKey)
        {
            var sequenceChanged = false;

            var currentIndex = _pressedSequence.Count;          // Index of the current press in the list (after it's added)
            var isCorrectKey = EvaluateCorrectness(pressedKey);
            KeyCorrectness correctness;

            if (isCorrectKey)
            {
                _pressedSequence.Add(pressedKey);
                SetCorrectness(currentIndex, true);
                sequenceChanged = true;
                correctness = KeyCorrectness.Correct;
            }
            else
            {
                var isBackspace =
                    pressedKey.HardwareKey == HardwareKey.Backspace &&
                    pressedKey.Modifiers == Modifier.None;
                var isOtherKeyPressedPreviously = PressedSequence.Count > 0;
                var isBackspaceToDeleteLastEntry = isBackspace && isOtherKeyPressedPreviously;

                if (isBackspaceToDeleteLastEntry && AllowCorrections)
                {
                    var indexToRemove = _pressedSequence.Count - 1;
                    SetCorrectness(indexToRemove, true);
                    correctness = KeyCorrectness.Indeterminate;
                    _pressedSequence.RemoveAt(indexToRemove);
                    sequenceChanged = true;
                }
                else
                {
                    // Special case: if only modifier keys are pressed, ignore it and don't count it as a wrong input
                    var onlyModifiers = pressedKey.HardwareKey.IsModifier();
                    if (!onlyModifiers)
                    {
                        // Add it (wrong input input...)
                        _pressedSequence.Add(pressedKey);
                        SetCorrectness(currentIndex, false);
                        sequenceChanged = true;
                        correctness = KeyCorrectness.Wrong;
                    }
                    else
                    {
                        correctness = KeyCorrectness.Indeterminate;
                    }
                }
            }

            if (sequenceChanged)
            {
                PressedSequenceChanged?.Invoke(this);
            }

            return correctness;
        }

        private bool EvaluateCorrectness(IHardwareKeyMappingTarget pressedKey)
        {
            var isCorrect = pressedKey.Equals(NextExpectedMappingTarget);
            return isCorrect;
        }

        private void SetCorrectness(int mappingTargetIndex, bool isCorrect)
        {
            if (isCorrect)
            {
                _errorIndexes.Remove(mappingTargetIndex);
            }
            else
            {
                if (!_errorIndexes.Contains(mappingTargetIndex))
                {
                    _errorIndexes.Add(mappingTargetIndex);
                    _errorIndexes.Sort();
                }
            }
        }

    }
}