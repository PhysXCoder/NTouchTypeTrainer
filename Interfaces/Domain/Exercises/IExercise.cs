using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Interfaces.Domain.Exercises
{
    public interface IExercise : IEnumerable<IMappingTarget>
    {
        ExerciseType ExerciseType { get; }

        IReadOnlyDictionary<CultureInfo, string> Descriptions { get; }

        IEnumerable<IMappingTarget> ExpectedSequence { get; }

        void BuildExpectedSequence();
    }
}