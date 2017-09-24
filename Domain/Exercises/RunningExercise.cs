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

        public IReadOnlyList<IMappingTarget> PressedSequence
            => _pressedSequence.AsReadOnly();

        public int NextExpectedIndex
            => _pressedSequence.Count;

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
            Exercise = exercise;
        }

        public void EvaluateKeyUp(IHardwareKeyMappingTarget pressedKey)
        {
            var sequenceChanged = false;

            if (pressedKey.Equals(NextExpectedMappingTarget))
            {
                _pressedSequence.Add(pressedKey);
                sequenceChanged = true;
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
                    _pressedSequence.RemoveAt(_pressedSequence.Count - 1);
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
                        sequenceChanged = true;
                    }
                }
            }

            if (sequenceChanged)
            {
                PressedSequenceChanged?.Invoke(this);
            }
        }

    }
}