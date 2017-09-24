using NTouchTypeTrainer.Domain.Keyboard.Keys;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard
{
    public interface IMechanicalKeyboardLayout
    {
        int NumberOfRows { get; }

        int IndexMinRow { get; }

        int IndexMaxRow { get; }

        IReadOnlyDictionary<KeyPosition, float?> KeySizefactors { get; }

        int GetNumberOfKeysInRow(int iRow);

        float? GetSizefactor(KeyPosition position);
    }
}