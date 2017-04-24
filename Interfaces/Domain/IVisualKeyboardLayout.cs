using NTouchTypeTrainer.Domain;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IVisualKeyboardLayout
    {
        IReadOnlyDictionary<PressedKey, IMappedKey> KeyMappings { get; }

        IReadOnlyDictionary<IMappedKey, PressedKey> ReverseKeyMappings { get; }
    }
}