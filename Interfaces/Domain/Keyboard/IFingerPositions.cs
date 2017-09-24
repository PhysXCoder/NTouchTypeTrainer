using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard.Keys;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard
{
    public interface IFingerPositions
    {
        Finger this[KeyPosition key] { get; }

        bool ContainsKey(KeyPosition keyPosition);

        IEnumerable<KeyValuePair<KeyPosition, Finger>> GetAllKeyFingerPairs();
    }
}