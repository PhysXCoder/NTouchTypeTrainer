using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard
{
    public interface IVisualKeyboardLayout
    {
        IReadOnlyDictionary<IKeyboardKey, IMappingTarget> Map { get; }

        IReadOnlyDictionary<IMappingTarget, IKeyboardKey> ReverseMap { get; }
    }
}