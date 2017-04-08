using System;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Contracts.Enums
{
    [Flags]
    public enum Modifier
    {
        None = 0,
        Shift = 1,
        AltGr = 2,
        Ctrl = 4,

        All = None | Shift | AltGr | Ctrl
    }

    public static class ModifierExtension
    {
        public static IEnumerable<Modifier> GetAllCombinations(this Modifier maxModifier)
        {
            var range = Enumerable
                .Range((int)Modifier.None, (int)maxModifier)
                .Select(m => (Modifier)m);

            return range;
        }
    }
}

