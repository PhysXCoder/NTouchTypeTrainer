using System.Collections.Generic;
using System.Linq;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public abstract class KeyboardLayoutBasePorter : BasePorter
    {
        protected static readonly IEnumerable<Modifier?> AllModifiers =
            Modifier.All.GetAllCombinations().Select(m => (Modifier?)m);

        protected const string MappingNameSeparator = ":" + NewLine;

        protected static string GetModifierStartToken(Modifier modifier)
        {
            return modifier + MappingNameSeparator;
        }
        protected static bool ContainsMapping(IKeyboardLayout layout, Modifier modifier)
        {
            return layout.AllRows.Any(km => km.Mappings.ContainsKey(modifier));
        }
    }
}