namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IKeyboardKeyMapping
    {
        IKeyboardKey KeyboardKey { get; }

        IMappingTarget MappedKey { get; }
    }
}

