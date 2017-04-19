using System.Text;

namespace NTouchTypeTrainer.Common.Strings
{
    public static class StringBuilderExtensions
    {
        public static void RemoveLast(this StringBuilder builder, string toRemove)
        {
            if (builder.ToString().EndsWith(toRemove))
            {
                builder.Remove(builder.Length - toRemove.Length, toRemove.Length);
            }
        }
    }
}