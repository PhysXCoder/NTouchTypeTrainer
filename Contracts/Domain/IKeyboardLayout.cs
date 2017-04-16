using System.Collections.Generic;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IKeyboardLayout
    {
        IReadOnlyList<IKeyMapping> DigitsRow { get; }

        IReadOnlyList<IKeyMapping> UpperCharacterRow { get; }

        IReadOnlyList<IKeyMapping> MiddleCharacterRow { get; }

        IReadOnlyList<IKeyMapping> LowerCharacterRow { get; }

        IReadOnlyList<IKeyMapping> ControlKeyRow { get; }

        IReadOnlyList<IKeyMapping> AllRows { get; }

    }
}