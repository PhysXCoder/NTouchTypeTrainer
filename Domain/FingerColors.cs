using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer.Domain
{
    public class FingerColors : IFingerColors, IStringExport, IStringImport<FingerColors>
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

        private readonly FingerColorsExporter _fingerColorsExporter;
        private readonly FingerColorsImporter _fingerColorsImporter;

        public FingerColors()
        {
            _fingerColorsExporter = new FingerColorsExporter();
            _fingerColorsImporter = new FingerColorsImporter();

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

        public string Export() => _fingerColorsExporter.Export(this);

        public bool TryImport(string exportedString, out FingerColors outputInstance) =>
            _fingerColorsImporter.TryImport(exportedString, out outputInstance);

        public FingerColors Import(string exportedString) =>
            _fingerColorsImporter.Import(exportedString);

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