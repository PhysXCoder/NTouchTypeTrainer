using System.Collections.Generic;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IKeyMapping
    {
        HardwareKey PressedKey { get; }

        IReadOnlyDictionary<Modifier, IMappedKey> Mappings { get; }

        string Export(Modifier modifier);
    }
}

