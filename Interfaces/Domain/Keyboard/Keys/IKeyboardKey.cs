using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard.Keys;
using System;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys
{
    public interface IKeyboardKey : IEquatable<KeyboardKey>
    {
        KeyPosition KeyPosition { get; }

        Modifier Modifiers { get; }
    }
}