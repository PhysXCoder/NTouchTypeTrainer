using NTouchTypeTrainer.Interfaces.Common;
using System;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Common.DataStructures
{
    public class Range<TIndex> : IEquatable<Range<TIndex>>, IImmutable
        where TIndex : IEquatable<TIndex>
    {
        /// <summary>
        /// Index where to start (including the start)
        /// </summary>
        public TIndex Start { get; }

        /// <summary>
        /// Index where to end (including the end)
        /// </summary>
        public TIndex Stop { get; }

        public Range(TIndex start, TIndex stop)
        {
            Start = start;
            Stop = stop;
        }

        public override string ToString()
            => $"[{Start}; {Stop}]";

        public bool Equals(Range<TIndex> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TIndex>.Default.Equals(Start, other.Start) &&
                EqualityComparer<TIndex>.Default.Equals(Stop, other.Stop);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Range<TIndex>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TIndex>.Default.GetHashCode(Start) * 397) ^
                    EqualityComparer<TIndex>.Default.GetHashCode(Stop);
            }
        }

    }
}

