using System;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace NTouchTypeTrainer.ViewModels
{
    public abstract class BaseViewModel : PropertyChangedBase
    {
        protected void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
            where T : IEquatable<T>
        {
            var areEqual = (backingField != null) ? backingField.Equals(value) : (value == null);
            if (!areEqual)
            {
                backingField = value;
                NotifyOfPropertyChange(propertyName);
            }
        }
    }
}