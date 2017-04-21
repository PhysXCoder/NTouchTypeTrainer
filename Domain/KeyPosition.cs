using NTouchTypeTrainer.Contracts.Common;
using System;

namespace NTouchTypeTrainer.Domain
{
    public struct KeyPosition : IEquatable<KeyPosition>, IImmutable
    {
        public int Row { get; }
        public int Key { get; }

        public KeyPosition(int row, int key)
        {
            Row = row;
            Key = key;
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