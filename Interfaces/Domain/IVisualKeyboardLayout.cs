using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IVisualKeyboardLayout
    {
        IReadOnlyDictionary<IKeyboardKey, IMappingTarget> KeyMappings { get; }

        IReadOnlyDictionary<IMappingTarget, IKeyboardKey> ReverseKeyMappings { get; }
    }
}