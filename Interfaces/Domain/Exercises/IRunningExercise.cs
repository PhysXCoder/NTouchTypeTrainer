using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain.Exercises
{
    public interface IRunningExercise
    {
        IExercise Exercise { get; }

        IReadOnlyList<IMappingTarget> PressedSequence { get; }

        IReadOnlyList<int> ErrorIndexes { get; }

        IMappingTarget NextExpectedMappingTarget { get; }

        int NextExpectedIndex { get; }

        void EvaluateKeyUp(IHardwareKeyMappingTarget pressedKey);

        event Action<IRunningExercise> PressedSequenceChanged;
    }
}