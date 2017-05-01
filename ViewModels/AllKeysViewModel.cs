using Eto.Drawing;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#pragma warning disable 659
namespace NTouchTypeTrainer.ViewModels
{
    public class AllKeysViewModel : BaseViewModel, IEquatable<AllKeysViewModel>
    {
        private ObservableDictionary<KeyPosition, KeyViewModel> _keys;

        private IFingerPositions _fingerPositions;
        private IFingerColors _fingerColors;
        private IVisualKeyboardLayout _visualKeyboardLayout;

        public ObservableDictionary<KeyPosition, KeyViewModel> Keys
        {
            get => _keys;
            set
            {
                _keys = value;
                OnPropertyChanged();
            }
        }

        public KeyViewModel this[IMappedKey key]
            => Keys[_visualKeyboardLayout.ReverseKeyMappings[key].KeyPosition];

        public AllKeysViewModel()
        {
            _fingerColors = new FingerColors();
            _keys = new ObservableDictionary<KeyPosition, KeyViewModel>();
            _visualKeyboardLayout = new VisualKeyboardLayout(new List<IPressedKeyMapping>());
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

        public void LoadVisualKeyboardLayout(IVisualKeyboardLayout layout)
        {
            _visualKeyboardLayout = layout;
            UpdateAllKeys();
        }

        public void LoadMechanicalKeyboardLayout(IMechanicalKeyboardLayout keyboardLayout)
        {
            var toRemove = new List<KeyPosition>(Keys.Keys);

            for (int iRow = keyboardLayout.IndexMinRow; iRow <= keyboardLayout.IndexMaxRow; iRow++)
            {
                foreach (var keyPosition in keyboardLayout.KeySizes.Keys.Where(keyPos => (keyPos.Row == iRow)))
                {
                    if (Keys.Keys.Contains(keyPosition))
                    {
                        toRemove.Remove(keyPosition);
                    }
                    else
                    {
                        var viewModel = new KeyViewModel();
                        Keys.Add(keyPosition, viewModel);

                        UpdateKey(keyPosition, viewModel);
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
            foreach (var keyPosition in Keys.Keys)
            {
                UpdateKey(keyPosition, Keys[keyPosition]);
            }
        }
        private void UpdateKey(KeyPosition keyPosition, KeyViewModel keyViewModel)
        {
            var finger = GetFingerForKeyPosition(keyPosition);

            var mappedKey = _visualKeyboardLayout?.KeyMappings
                ?.Where(mapping => mapping.Key.KeyPosition.Equals(keyPosition))
                .OrderBy(mapping => (int)mapping.Key.Modifiers)
                .Select(mapping => mapping.Value)
                .FirstOrDefault();

            var mappedUnprintable = mappedKey as MappedUnprintable;
            var mappingTargetName = (mappedUnprintable != null) ?
                mappedUnprintable.Key.GetDefaultText()
                : mappedKey?.Name;

            keyViewModel.Name = mappingTargetName ?? "";
            keyViewModel.HighlightedColor = GetColor(finger);
        }

        private Finger? GetFingerForKeyPosition(KeyPosition keyPosition)
        {
            return _fingerPositions.ContainsKey(keyPosition) ? (Finger?)_fingerPositions[keyPosition] : null;
        }

        private Color GetColor(Finger? finger)
        {
            return _fingerColors[finger];
        }
    }
}