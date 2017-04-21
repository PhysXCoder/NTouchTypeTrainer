using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eto.Drawing;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;

#pragma warning disable 659
namespace NTouchTypeTrainer.ViewModels
{
    public class AllKeysViewModel : BaseViewModel, IEquatable<AllKeysViewModel>
    {
        private ObservableDictionary<HardwareKey, KeyViewModel> _keys;

        private IFingerPositions _fingerPositions;
        private IFingerColors _fingerColors;

        public ObservableDictionary<HardwareKey, KeyViewModel> Keys
        {
            get => _keys;
            set
            {
                _keys = value;
                OnPropertyChanged();
            }
        }

        public AllKeysViewModel()
        {
            _fingerColors = new FingerColors();
            _keys = new ObservableDictionary<HardwareKey, KeyViewModel>();
        }

        public void LoadFingerColors(IFingerColors fingerColors)
        {
            _fingerColors = fingerColors;
            UpdateAllKeys();
        }

        public void LoadFingerPositions(IFingerPositions fingerPositions)
        {
            _fingerPositions = fingerPositions;
            UpdateAllKeys();
        }

        public void LoadMechanicalKeyboardLayout(IMechanicalKeyboardLayout keyboardLayout)
        {
            var toRemove = new List<HardwareKey>(Keys.Keys);

            foreach (var keyboardRow in keyboardLayout.KeyboardRows)
            {
                foreach (var hardwareKey in keyboardRow)
                {
                    if (Keys.Keys.Contains(hardwareKey))
                    {
                        toRemove.Remove(hardwareKey);
                    }
                    else
                    {
                        var viewModel = new KeyViewModel();
                        Keys.Add(hardwareKey, viewModel);

                        UpdateKey(hardwareKey, viewModel);
                    }
                }
            }

            foreach (var hardwareKey in toRemove)
            {
                Keys.Remove(hardwareKey);
            }
        }

        public bool Equals(AllKeysViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return GetAllKeyViewModels().All(prop =>
                prop.GetValue(this).Equals(prop.GetValue(other)));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AllKeysViewModel)obj);
        }

        private IEnumerable<PropertyInfo> GetAllKeyViewModels()
        {
            return typeof(AllKeysViewModel)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.PropertyType == typeof(KeyViewModel));
        }

        private void UpdateAllKeys()
        {
            foreach (var hardwareKey in Keys.Keys)
            {
                UpdateKey(hardwareKey, Keys[hardwareKey]);
            }
        }
        private void UpdateKey(HardwareKey key, KeyViewModel keyViewModel)
        {
            var finger = GetFingerForKey(key);

            keyViewModel.Name = key.GetDefaultText();
            keyViewModel.HighlightedColor = GetColor(finger);
        }

        private Finger? GetFingerForKey(HardwareKey key)
        {
            return _fingerPositions.ContainsKey(key) ? (Finger?)_fingerPositions[key] : null;
        }

        private Color GetColor(Finger? finger)
        {
            return _fingerColors[finger];
        }
    }
}