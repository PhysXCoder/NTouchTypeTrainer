using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain.Exercises
{
    public class ExerciseBlock : IImmutable
    {
        public int NumberOfRows { get; }

        public int NumberOfGroupsPerRow { get; }

        public IReadOnlyList<IReadOnlyList<IMappingTarget>> Groups { get; }

        public ExerciseBlock(int rows, int groupsInRow, IEnumerable<IEnumerable<IMappingTarget>> groups)
        {
            NumberOfRows = rows;
            NumberOfGroupsPerRow = groupsInRow;
            Groups = groups
                .Select(gr => gr.ToList().AsReadOnly())
                .ToList()
                .AsReadOnly();
        }
    }
}