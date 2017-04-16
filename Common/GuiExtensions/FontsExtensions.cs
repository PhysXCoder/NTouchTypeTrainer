using Eto.Drawing;

namespace NTouchTypeTrainer.Common.GuiExtensions
{
    public static class FontsExtensions
    {
        public static Font Inflate(this Font font, float enlargement)
        {
            return new Font(font.Family, font.Size + enlargement, font.FontStyle, font.FontDecoration);
        }
    }
}