using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Common.Graphics;

namespace NTouchTypeTrainer.Common.Graphics
{
    public class GraphicsProvider : IGraphicsProvider
    {
        public Eto.Drawing.Graphics Graphics { get; }

        public GraphicsProvider()
        {
            var bitmap = new Bitmap(new Size(1, 1), PixelFormat.Format32bppRgba);
            Graphics = new Eto.Drawing.Graphics(bitmap);
        }
    }
}