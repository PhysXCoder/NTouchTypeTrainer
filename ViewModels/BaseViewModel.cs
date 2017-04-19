using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NTouchTypeTrainer.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
            where T : IEquatable<T>
        {
            var areEqual = (backingField != null) ? backingField.Equals(value) : (value == null);

            if (!areEqual)
            {
                backingField = value;

                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(propertyName);
            }
        }
    }
}