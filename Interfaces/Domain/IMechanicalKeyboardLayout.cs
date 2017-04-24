using NTouchTypeTrainer.Domain;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IMechanicalKeyboardLayout
    {
        int NumberOfRows { get; }

        int IndexMinRow { get; }

        int IndexMaxRow { get; }

        IReadOnlyDictionary<KeyPosition, float?> KeySizes { get; }

        int GetNumberOfKeysInRow(int iRow);

        float? GetSize(KeyPosition position);
    }
}