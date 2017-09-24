using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;

namespace NTouchTypeTrainer.Domain.Keyboard.Keys
{
    /// <summary>
    /// Represents a pressed key on the keyboard, including modifier keys.
    /// </summary>
    public class KeyboardKey : IImmutable, IKeyboardKey
    {
        public KeyPosition KeyPosition { get; }

        public Modifier Modifiers { get; }

        public KeyboardKey(KeyPosition position, Modifier modifiers)
        {
            KeyPosition = position;
            Modifiers = modifiers;
        }

        public bool Equals(KeyboardKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return KeyPosition.Equals(other.KeyPosition) && Modifiers == other.Modifiers;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyboardKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (KeyPosition.GetHashCode() * 397) ^ (int)Modifiers;
            }
        }

        public override string ToString()
            => $"KeyboardKey {{Pos: {KeyPosition}, Modifiers: {Modifiers}}}";
    }
}