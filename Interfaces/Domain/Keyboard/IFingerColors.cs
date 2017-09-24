using NTouchTypeTrainer.Domain.Enums;
using System.Windows.Media;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard
{
    public interface IFingerColors
    {
        Color DefaultColor { get; }

        Color this[Finger? finger] { get; }
    }
}