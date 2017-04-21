using System.Collections.Generic;
using System.Linq;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public abstract class KeyboardLayoutBasePorter : BasePorter
    {
        protected static readonly IEnumerable<Modifier?> AllModifiers =
            Modifier.All.GetAllCombinations().Select(m => (Modifier?)m);

        protected const string MappingNameSeparator = Separator + NewLine;

        protected static string GetModifierStartToken(Modifier modifier)
        {
            return modifier + MappingNameSeparator;
        }
    }
}