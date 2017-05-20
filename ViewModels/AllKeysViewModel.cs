using Eto.Drawing;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Domain;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.ViewModels
{
    public class AllKeysViewModel : BaseViewModel
    {
        private ObservableDictionary<KeyPosition, KeyViewModel> _keysByPosition;

        private IFingerPositions _fingerPositions;
        private IFingerColors _fingerColors;
        private IVisualKeyboardLayout _visualKeyboardLayout;

        public ObservableDictionary<KeyPosition, KeyViewModel> KeysByPosition
        {
            get => _keysByPosition;
            set
            {
                _keysByPosition = value;
                OnPropertyChanged();
            }
        }

        public KeyViewModel this[IMappingTarget target]
            => KeysByPosition[_visualKeyboardLayout.ReverseKeyMappings[target].KeyPosition];

        public AllKeysViewModel()
        {
            _fingerColors = new FingerColors();
            _keysByPosition = new ObservableDictionary<KeyPosition, KeyViewModel>();
            _visualKeyboardLayout = new VisualKeyboardLayout(new List<IKeyboardKeyMapping>());
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
            var toRemove = new List<KeyPosition>(KeysByPosition.Keys);

            for (int iRow = keyboardLayout.IndexMinRow; iRow <= keyboardLayout.IndexMaxRow; iRow++)
            {
                foreach (var keyPosition in keyboardLayout.KeySizes.Keys.Where(keyPos => (keyPos.Row == iRow)))
                {
                    if (KeysByPosition.Keys.Contains(keyPosition))
                    {
                        toRemove.Remove(keyPosition);
                    }
                    else
                    {
                        var viewModel = new KeyViewModel();
                        KeysByPosition.Add(keyPosition, viewModel);

                        UpdateKey(keyPosition, viewModel);
                    }
                }
            }

            foreach (var hardwareKey in toRemove)
            {
                KeysByPosition.Remove(hardwareKey);
            }
        }

        private void UpdateAllKeys()
        {
            foreach (var keyPosition in KeysByPosition.Keys)
            {
                UpdateKey(keyPosition, KeysByPosition[keyPosition]);
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

            var mappedUnprintable = mappedKey as MappedHardwareKey;
            var mappingTargetName = (mappedUnprintable != null) ?
                mappedUnprintable.HardwareKey.GetDefaultText()
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