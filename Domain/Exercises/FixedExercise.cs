using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public class FixedExercise : Exercise, IImmutable
    {
        private readonly IReadOnlyList<IMappingTarget> _sequence;

        public override ExerciseType ExerciseType
            => ExerciseType.Fix;

        public override IEnumerable<IMappingTarget> Sequence
            => _sequence;

        public FixedExercise(IDictionary<CultureInfo, string> descriptions, IEnumerable<IMappingTarget> keys)
            : base(descriptions)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            _sequence = new List<IMappingTarget>(keys).AsReadOnly();
        }

        public override void BuildSequence()
        {
            // Sequence is fixed and stays the same. No need to do anything in this Method.
        }

        public override IEnumerator<IMappingTarget> GetEnumerator()
            => _sequence.GetEnumerator();
    }
}