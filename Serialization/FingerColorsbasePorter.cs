using System.Linq;
using System.Reflection;
using Eto.Drawing;
using NTouchTypeTrainer.Domain;

namespace NTouchTypeTrainer.Serialization
{
    public abstract class FingerColorsBasePorter : BasePorter
    {
        protected const string NameColorSeparator = ": ";

        protected static PropertyInfo[] GetColorProperties() =>
            typeof(FingerColors).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color))
                .ToArray();
    }
}