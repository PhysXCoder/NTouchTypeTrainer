using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using System;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IKeyboardKey : IEquatable<KeyboardKey>
    {
        KeyPosition KeyPosition { get; }

        Modifier Modifiers { get; }
    }
}