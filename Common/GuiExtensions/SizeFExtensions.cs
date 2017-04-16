using Eto.Drawing;

namespace NTouchTypeTrainer.Common.GuiExtensions
{
    public static class SizeFExtensions
    {
        public static SizeF Inflate(this SizeF size, SizeF enlargement)
            => new SizeF(new PointF(size + enlargement));

        public static SizeF Inflate(this SizeF size, int enlargement)
            => new SizeF(new PointF(size + new SizeF(enlargement, enlargement)));

        public static SizeF Inflate(this SizeF size, float enlargement)
            => new SizeF(new PointF(size + new SizeF(enlargement, enlargement)));
    }
}