using NTouchTypeTrainer.Common.Serialization;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;

namespace NTouchTypeTrainer.Domain.Keyboard.Keys
{
    /// <summary>
    /// Represents a keyboard key and its mapped character/hadrware-key object.
    /// </summary>
    public class KeyMapping : BaseImporter, IKeyboardKeyMapping, IImmutable
    {
        public IKeyboardKey KeyboardKey { get; }

        public IMappingTarget MappingTarget { get; }

        public KeyMapping(IKeyboardKey keyboardKey, IMappingTarget mappedTarget)
        {
            KeyboardKey = keyboardKey;
            MappingTarget = mappedTarget;
        }
    }
}