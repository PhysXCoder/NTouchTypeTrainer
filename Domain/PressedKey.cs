using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using System;

namespace NTouchTypeTrainer.Domain
{
    public struct PressedKey : IEquatable<PressedKey>, IImmutable
    {
        public KeyPosition KeyPosition { get; }

        public Modifier Modifiers { get; }

        public PressedKey(KeyPosition position, Modifier modifiers)
        {
            KeyPosition = position;
            Modifiers = modifiers;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return (obj is PressedKey) && Equals((PressedKey)obj);
        }

        public bool Equals(PressedKey other)
        {
            return KeyPosition.Equals(other.KeyPosition) && (Modifiers == other.Modifiers);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (KeyPosition.GetHashCode() * 397) ^ (int)Modifiers;
            }
        }

        public override string ToString()
            => $"PressedKey {{Pos: {KeyPosition}, Modifiers: {Modifiers}}}";
    }
}