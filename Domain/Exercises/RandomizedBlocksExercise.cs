using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public class RandomizedBlocksExercise : Exercise
    {
        protected readonly IMappingTarget GroupSeparator = new CharacterMappingTarget(' ');
        protected readonly IMappingTarget RowSeparator = new HardwareKeyMappingTarget(HardwareKey.Enter);

        private readonly IList<ExerciseBlock> _exerciseBlocks;
        private readonly Random _random;

        private IReadOnlyList<IMappingTarget> _currentKeySequence;

        public override ExerciseType ExerciseType
            => ExerciseType.RandomizedBlocks;

        public override IEnumerable<IMappingTarget> ExpectedSequence
        {
            get
            {
                if (_currentKeySequence == null)
                {
                    BuildExpectedSequence();
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

        public override void BuildExpectedSequence()
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