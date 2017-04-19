using Eto.Drawing;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IFingerColors
    {
        Color DefaultColor { get; }
        Color SmallLeftFingerColor { get; }
        Color RingLeftFingerColor { get; }
        Color MiddleLeftFingerColor { get; }
        Color IndexLeftFingerColor { get; }
        Color ThumbColor { get; }
        Color IndexRightFingerColor { get; }
        Color MiddleRightFingerColor { get; }
        Color RingRightFingerColor { get; }
        Color SmallRightFingerColor { get; }

        Color this[Finger? finger] { get; }
    }
}