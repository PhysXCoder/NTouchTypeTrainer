using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer.Domain
{
    public class FingerColors : IFingerColors, IStringImport<FingerColors>
    {
        public Color DefaultColor { get; private set; }

        public Color SmallLeftFingerColor { get; private set; }
        public Color RingLeftFingerColor { get; private set; }
        public Color MiddleLeftFingerColor { get; private set; }
        public Color IndexLeftFingerColor { get; private set; }

        public Color ThumbColor { get; private set; }

        public Color IndexRightFingerColor { get; private set; }
        public Color MiddleRightFingerColor { get; private set; }
        public Color RingRightFingerColor { get; private set; }
        public Color SmallRightFingerColor { get; private set; }

        public FingerColors()
        {
            SetDefaultColors();
        }

        private void SetDefaultColors()
        {
            DefaultColor = (new Button()).BackgroundColor;

            SmallLeftFingerColor = Colors.DeepSkyBlue;
            RingLeftFingerColor = Colors.Turquoise;
            MiddleLeftFingerColor = Color.FromArgb(0x00, 0xe0, 0x40);
            IndexLeftFingerColor = Colors.GreenYellow;

            ThumbColor = Colors.Gray;

            IndexRightFingerColor = Colors.Yellow;
            MiddleRightFingerColor = Colors.Orange;
            RingRightFingerColor = Colors.Red;
            SmallRightFingerColor = Color.FromArgb(0xb0, 0x70, 0xff);
        }

        public bool TryImport(string exportedString, out FingerColors outputInstance) =>
            FingerColorsImporter.TryImport(exportedString, out outputInstance);

        public FingerColors Import(string exportedString) =>
            FingerColorsImporter.Import(exportedString);

        public Color this[Finger? finger]
        {
            get
            {
                switch (finger)
                {
                    case Finger.SmallLeft:
                        return SmallLeftFingerColor;
                    case Finger.RingLeft:
                        return RingLeftFingerColor;
                    case Finger.MiddleLeft:
                        return MiddleLeftFingerColor;
                    case Finger.IndexLeft:
                        return IndexLeftFingerColor;
                    case Finger.Thumb:
                        return ThumbColor;
                    case Finger.IndexRight:
                        return IndexRightFingerColor;
                    case Finger.MiddleRight:
                        return MiddleRightFingerColor;
                    case Finger.RingRight:
                        return RingRightFingerColor;
                    case Finger.SmallRight:
                        return SmallRightFingerColor;

                    case null:
                        return DefaultColor;
                    default:
                        return DefaultColor;
                }
            }
        }
    }
}