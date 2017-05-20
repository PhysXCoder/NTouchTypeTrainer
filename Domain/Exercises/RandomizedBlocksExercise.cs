using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public class RandomizedBlocksExercise : Exercise
    {
        protected readonly IMappingTarget GroupSeparator = new MappedCharacter(' ');
        protected readonly IMappingTarget RowSeparator = new MappedHardwareKey(HardwareKey.Enter);

        private readonly IList<ExerciseBlock> _exerciseBlocks;
        private readonly Random _random;

        private IReadOnlyList<IMappingTarget> _currentKeySequence;

        public override ExerciseType ExerciseType
            => ExerciseType.RandomizedBlocks;

        public override IEnumerable<IMappingTarget> Sequence
        {
            get
            {
                if (_currentKeySequence == null)
                {
                    BuildSequence();
                }

                return _currentKeySequence;
            }
        }

        public RandomizedBlocksExercise(IDictionary<CultureInfo, string> descriptions, IEnumerable<ExerciseBlock> exerciseBlocks)
            : base(descriptions)
        {
            _random = new Random();
            _exerciseBlocks = new List<ExerciseBlock>(exerciseBlocks);
        }

        public override IEnumerator<IMappingTarget> GetEnumerator()
            => _currentKeySequence.GetEnumerator();

        public override void BuildSequence()
        {
            _currentKeySequence = GetNextKeySequence().AsReadOnly();
        }

        private List<IMappingTarget> GetNextKeySequence()
        {
            var keys = new List<IMappingTarget>();

            _exerciseBlocks.ForEach(exerciseBlock =>
            {
                exerciseBlock.NumberOfRows.Repeat(
                    repeat: () =>
                    {
                        exerciseBlock.NumberOfGroupsPerRow.Repeat(
                            repeat: () =>
                            {
                                keys.AddRange(GetRandomGroupText(exerciseBlock));
                                keys.Add(GroupSeparator);
                            },
                            ifRepeated: () => keys.RemoveLast(GroupSeparator)
                        );

                        keys.Add(RowSeparator);
                    },
                    ifRepeated: () => keys.RemoveLast(RowSeparator)
                );
            });

            return keys;
        }

        private IEnumerable<IMappingTarget> GetRandomGroupText(ExerciseBlock block)
        {
            var randomNumber = _random.Next(0, block.Groups.Count);
            return block.Groups[randomNumber];
        }
    }
}