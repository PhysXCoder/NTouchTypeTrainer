using NTouchTypeTrainer.Domain;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IPressedKeyMapping
    {
        PressedKey PressedKey { get; }

        IMappedKey MappedKey { get; }
    }
}

