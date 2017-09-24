using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys
{
    public interface IKeyboardKeyMapping
    {
        IKeyboardKey KeyboardKey { get; }

        IMappingTarget MappingTarget { get; }
    }
}

