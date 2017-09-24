using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets
{
    public interface IHardwareKeyMappingTarget : IMappingTarget
    {
        HardwareKey HardwareKey { get; }

        Modifier Modifiers { get; }
    }
}