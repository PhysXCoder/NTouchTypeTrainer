using Eto.Drawing;

namespace NTouchTypeTrainer.Common.GuiExtensions
{
    public static class SizeExtensions
    {
        public static Size Inflate(this Size size, Size enlargement)
            => new Size(size + enlargement);

        public static Size Inflate(this Size size, int enlargement)
            => new Size(size + new Size(enlargement, enlargement));
    }
}