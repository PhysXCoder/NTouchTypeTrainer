using NTouchTypeTrainer.Interfaces.Common;
using System;

namespace NTouchTypeTrainer.Domain
{
    public struct KeyPosition : IEquatable<KeyPosition>, IComparable<KeyPosition>, IComparable, IImmutable
    {
        public int Row { get; }

        public int Key { get; }

        public KeyPosition(int row, int key)
        {
            Row = row;
            Key = key;
        }

        public int CompareTo(object obj)
            => CompareTo((KeyPosition)obj);

        public int CompareTo(KeyPosition other)
        {
            if (Row != other.Row)
            {
                return Row - other.Row;
            }

            return Key - other.Key;
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
            return (Row == other.Row) && (Key == other.Key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Key;
            }
        }
    }
}