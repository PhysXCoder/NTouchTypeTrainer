using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class ModifierExtensions
    {
        private static IEnumerable<Modifier> GetAllCombinations(this Modifier maxModifier)
        {
            var range = Enumerable
                .Range((int)Modifier.None, (int)maxModifier)
                .Select(m => (Modifier)m);

            return range;
        }

        public static readonly IEnumerable<Modifier> AllModifiers =
            Modifier.All.GetAllCombinations().Select(m => m);
    }
}