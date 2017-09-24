using System;

namespace NTouchTypeTrainer.Common.Primitives
{
    public static class EnumExtensions
    {
        public static TEnum[] GetValues<TEnum>()
            => (TEnum[])Enum.GetValues(typeof(TEnum));
    }
}