namespace NTouchTypeTrainer.Common.Strings
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string str)
            => string.IsNullOrEmpty(str);
    }
}

