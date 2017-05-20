using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer.Domain
{
    public class KeyMapping : BaseImporter, IKeyboardKeyMapping, IImmutable
    {
        public IKeyboardKey KeyboardKey { get; }

        public IMappingTarget MappedKey { get; }

        public KeyMapping(IKeyboardKey keyboardKey, IMappingTarget mappedTarget)
        {
            KeyboardKey = keyboardKey;
            MappedKey = mappedTarget;
        }
    }
}