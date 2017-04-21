using NTouchTypeTrainer.Domain.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NTouchTypeTrainer.Serialization
{
    public abstract class MechanicalKeyboardLayoutBasePorter : BasePorter
    {
        protected const string RegularKey = "_";
        protected const string ProportionalKey = "~";
        protected static readonly NumberFormatInfo FloatFormat = CultureInfo.InvariantCulture.NumberFormat;
        protected const string MappingNameSeparator = Separator + NewLine;

        protected static readonly IEnumerable<Modifier?> AllModifiers =
            Modifier.All.GetAllCombinations().Select(m => (Modifier?)m);

        protected static string GetModifierStartToken(Modifier modifier)
        {
            return modifier + MappingNameSeparator;
        }
    }
}