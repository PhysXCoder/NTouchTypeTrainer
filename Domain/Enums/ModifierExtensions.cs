using NTouchTypeTrainer.Common.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class ModifierExtensions
    {
        public const string ModifierCombiner = "+";

        public static readonly IEnumerable<Modifier> AllModifiers =
            Enum.GetValues(typeof(Modifier))
                .Cast<Modifier>();

        public static readonly Modifier AllModifiersSet =
            Enum.GetValues(typeof(Modifier))
                .Cast<Modifier>()
                .Aggregate((mod1, mod2) => mod1 | mod2);

        public static readonly IEnumerable<Modifier> AllModifierCombinations =
            AllModifiersSet.GetAllCombinations().Select(m => m);

        public static string ToCombinedString(this Modifier modifiers)
        {
            var oneOreMoreIsSet = false;

            var builder = new StringBuilder();
            foreach (var currentModifier in AllModifiers)
            {
                var flagIsSet = (modifiers & currentModifier) > 0;
                if (flagIsSet)
                {
                    builder
                        .Append(currentModifier)
                        .Append(ModifierCombiner);
                    oneOreMoreIsSet = true;
                }
            }
            builder.RemoveLast(ModifierCombiner);

            return oneOreMoreIsSet ? builder.ToString() : null;
        }

        private static IEnumerable<Modifier> GetAllCombinations(this Modifier maxModifier)
        {
            var range = Enumerable
                .Range((int)Modifier.None, (int)maxModifier)
                .Select(m => (Modifier)m);

            return range;
        }
    }
}