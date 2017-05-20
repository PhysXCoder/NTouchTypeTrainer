using Eto.Drawing;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IFingerColors
    {
        Color DefaultColor { get; }

        Color this[Finger? finger] { get; }
    }
}