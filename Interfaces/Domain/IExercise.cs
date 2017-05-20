using NTouchTypeTrainer.Domain.Enums;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IExercise : IEnumerable<IMappingTarget>
    {
        ExerciseType ExerciseType { get; }

        IReadOnlyDictionary<CultureInfo, string> Descriptions { get; }

        IEnumerable<IMappingTarget> Sequence { get; }

        void BuildSequence();
    }
}