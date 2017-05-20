using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IMappedHardwareKey : IMappingTarget
    {
        Modifier Modifiers { get; }
    }
}