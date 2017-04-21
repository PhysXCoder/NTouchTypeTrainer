using System.Collections.Generic;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IMechanicalKeyboardLayout
    {
        IReadOnlyList<IReadOnlyList<HardwareKey>> KeyboardRows { get; }

        IReadOnlyList<HardwareKey> this[int iRow] { get; }

        HardwareKey this[int iRow, int iKey] { get; }
    }
}