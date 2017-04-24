using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer.Domain
{
    public class KeyMapping : BaseImporter, IPressedKeyMapping, IImmutable
    {
        public PressedKey PressedKey { get; }

        public IMappedKey MappedKey { get; }

        public KeyMapping(PressedKey pressedKey, IMappedKey mappedTarget)
        {
            PressedKey = pressedKey;
            MappedKey = mappedTarget;
        }
    }
}