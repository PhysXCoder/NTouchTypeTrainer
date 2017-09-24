using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain.Exercises;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public abstract class Exercise : IExercise
    {
        private readonly ReadOnlyDictionary<CultureInfo, string> _descriptions;

        public abstract ExerciseType ExerciseType { get; }

        public abstract IEnumerable<IMappingTarget> ExpectedSequence { get; }

        public IReadOnlyDictionary<CultureInfo, string> Descriptions
            => _descriptions;

        protected Exercise(IDictionary<CultureInfo, string> descriptions)
        {
            _descriptions = new ReadOnlyDictionary<CultureInfo, string>(descriptions);
        }

        public abstract IEnumerator<IMappingTarget> GetEnumerator();

        public abstract void BuildExpectedSequence();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}