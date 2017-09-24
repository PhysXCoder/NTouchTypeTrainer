using NTouchTypeTrainer.Interfaces.Common;
using System;

namespace NTouchTypeTrainer.Domain.Keyboard.Keys
{
    /// <summary>
    /// Represents the position of a key on the keyboard
    /// </summary>
    public struct KeyPosition : IEquatable<KeyPosition>, IComparable<KeyPosition>, IComparable, IImmutable
    {
        /// <summary>
        /// Row = 0 is the first row.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Index = 0 is the first key in a row.
        /// </summary>
        public int Index { get; }

        public KeyPosition(int row, int index)
        {
            Row = row;
            Index = index;
        }

        public int CompareTo(object obj)
            => CompareTo((KeyPosition)obj);

        public int CompareTo(KeyPosition other)
        {
            if (Row != other.Row)
            {
                return Row - other.Row;
            }

            return Index - other.Index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return (obj is KeyPosition) && Equals((KeyPosition)obj);
        }

        public bool Equals(KeyPosition other)
        {
            return (Row == other.Row) && (Index == other.Index);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Index;
            }
        }

        public override string ToString()
            => $"{nameof(KeyPosition)} {{#Row: {Row}, #Key: {Index}}}";
    }
}