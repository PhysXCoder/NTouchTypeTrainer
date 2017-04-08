using System.Collections.Generic;
using System.Linq;
using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public abstract class KeyboardLayoutBasePorter
    {
        protected static readonly IEnumerable<Modifier?> AllModifiers =
            Modifier.All.GetAllCombinations().Select(m => (Modifier?)m);

        protected const string NewLine = "\r\n";
        protected const string KeySeparator = " ";
        protected const string RowSeparator = NewLine;
        protected const string MappingNameSeparator = ":" + NewLine;

        protected static string GetModifierStartToken(Modifier modifier)
        {
            return modifier.ToString() + MappingNameSeparator;
        }
        protected static bool ContainsMapping(IKeyboardLayout layout, Modifier modifier)
        {
            return layout.AllRows.Any(km => km.Mappings.ContainsKey(modifier));
        }
    }

}