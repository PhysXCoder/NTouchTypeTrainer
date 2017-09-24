using Caliburn.Micro;
using System;

namespace NTouchTypeTrainer.Common.DataBinding
{
#pragma warning disable CS0659 
    public class BindableEquatableCollection<T> : BindableCollection<T>, IEquatable<BindableEquatableCollection<T>>
        where T : IEquatable<T>
    {
        public bool Equals(BindableEquatableCollection<T> other)
        {
            if (other == null || other.Count != Count)
            {
                return false;
            }

            for (var i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BindableEquatableCollection<T>)obj);
        }
    }
#pragma warning restore CS0659
}