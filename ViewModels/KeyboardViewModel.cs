using Caliburn.Micro;
using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard;
using NTouchTypeTrainer.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.Views.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyboardViewModel : BaseViewModel, IEnumerable<KeyViewModel>
    {
        private readonly IThemeProvider _themeProvider;
        private readonly ISizeGroup _sizeGroup;
        private readonly IEventAggregator _eventAggregator;
        private IFingerPositions _fingerPositions;
        private IFingerColors _fingerColors;
        private IVisualKeyboardLayout _visualKeyboardLayout;

        private ObservableCollection<KeyRowViewModel> _rows;

        public ObservableCollection<KeyRowViewModel> Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                NotifyOfPropertyChange();
            }
        }

        public KeyViewModel this[IMappingTarget mappedTarget]
        {
            get
            {
                var matchingKey = Rows
                    .SelectMany(row => row.Keys)
                    .FirstOrDefault(key =>
                        (key.Mappings != null) &&
                        key.Mappings.Any(mapping => mappedTarget.Equals(mapping.Value)));

                return matchingKey;
            }
        }

        public KeyboardViewModel(IThemeProvider themeProvider, ISizeGroup sizeGroup, IEventAggregator eventAggregator)
        {
            _themeProvider = themeProvider;
            _sizeGroup = sizeGroup;
            _eventAggregator = eventAggregator;     // ToDo: Probably not necessary anymore when objects get instantiated via Claiburn's IoC

            _fingerColors = new FingerColors(_themeProvider);
            _visualKeyboardLayout = new VisualKeyboardLayout(new List<IKeyboardKeyMapping>());

            _rows = new ObservableCollection<KeyRowViewModel>();
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

        public void UnhighlightAllKeys()
        {
            Rows.ForEach(row =>
            {
                row.Keys.ForEach(key => key.IsHighlighted = false);
            });
        }

        public void LoadMechanicalLayout(IMechanicalKeyboardLayout mechanicalLayout)
        {
            // Remove rows that don't exist in the new layout
            var iMinRow = mechanicalLayout.IndexMinRow;
            var iMaxRow = mechanicalLayout.IndexMaxRow;
            Rows
                .Where(row => row.RowIndex < iMinRow || row.RowIndex > iMaxRow)
                .ForEach(row => Rows.Remove(row));

            for (var iRow = iMinRow; iRow <= iMaxRow; iRow++)
            {
                var rowViewModel = GetOrCreateKeyRowViewModel(iRow);

                var keyposSizefactorPairs = GetKeyposSizefactorPairs(mechanicalLayout, iRow).ToList();
                RemoveKeysNotInLayout(rowViewModel, keyposSizefactorPairs.Select(pair => pair.Key));

                foreach (var pair in keyposSizefactorPairs)
                {
                    var keyViewModel = GetOrCreateKeyViewModel(rowViewModel: rowViewModel, keypos: pair.Key);
                    AddKeyMappingsToKeyViewModel(keyViewModel: keyViewModel, keypos: pair.Key, layout: _visualKeyboardLayout);
                    keyViewModel.SizeGroup = CreateScaledWidthSizeGroup(sizeFactor: pair.Value);
                }
            }
        }

        private KeyRowViewModel GetOrCreateKeyRowViewModel(int iRow)
        {
            var rowViewModel = Rows.FirstOrDefault(row => row.RowIndex == iRow);
            if (rowViewModel != null) return rowViewModel;

            rowViewModel = new KeyRowViewModel() { RowIndex = iRow };
            Rows.Insert(iRow, rowViewModel);
            return rowViewModel;
        }

        private static IEnumerable<KeyValuePair<KeyPosition, float?>> GetKeyposSizefactorPairs(
            IMechanicalKeyboardLayout mechanicalLayout, int currentRowIndex)
        {
            var keyposSizefactorPairs = mechanicalLayout.KeySizefactors
                .Where(keyposSizePair => (keyposSizePair.Key.Row == currentRowIndex))
                .ToList();

            return keyposSizefactorPairs;
        }

        private static void RemoveKeysNotInLayout(KeyRowViewModel currentRowViewModel, IEnumerable<KeyPosition> keyPositions)
        {
            var positionList = keyPositions.ToList();

            var iMinKey = positionList.Min(pos => pos.Index);
            var iMaxKey = positionList.Max(pos => pos.Index);

            currentRowViewModel.Keys
                .Where(key => key.KeyPosition.Index < iMinKey || key.KeyPosition.Index > iMaxKey)
                .ForEach(key => currentRowViewModel.Keys.Remove(key));
        }

        private KeyViewModel GetOrCreateKeyViewModel(KeyRowViewModel rowViewModel, KeyPosition keypos)
        {
            var keyViewModel = rowViewModel.Keys.FirstOrDefault(key => key.KeyPosition.Equals(keypos));
            if (keyViewModel != null) return keyViewModel;

            keyViewModel = new KeyViewModel(_themeProvider, _sizeGroup, _eventAggregator)
            {
                KeyPosition = keypos
            };
            rowViewModel.Keys.Insert(keypos.Index, keyViewModel);

            return keyViewModel;
        }

        private static void AddKeyMappingsToKeyViewModel(KeyViewModel keyViewModel, KeyPosition keypos, IVisualKeyboardLayout layout)
        {
            keyViewModel.Mappings = new Dictionary<IKeyboardKey, IMappingTarget>();
            if (layout == null) return;

            var keyMappings = layout.Map
                .Where(keyMappingPair => keyMappingPair.Key.KeyPosition.Equals(keypos));

            keyMappings.ForEach(mapping => keyViewModel.Mappings.Add(mapping));
        }

        private ISizeGroup CreateScaledWidthSizeGroup(float? sizeFactor)
        {
            ISizeGroup sizeGroup;

            if (sizeFactor.HasValue)
            {
                sizeGroup = new DependentSizeGroup(
                    _sizeGroup,
                    size => new Size(size.Width * sizeFactor.Value, size.Height),
                    size => new Size(size.Width / sizeFactor.Value, size.Height),
                    _eventAggregator);
            }
            else
            {
                sizeGroup = _sizeGroup;
            }

            return sizeGroup;
        }

        private void UpdateAllKeys()
        {
            Rows.ForEach(row =>
            {
                row.Keys.ForEach(UpdateKey);
            });
        }

        private void UpdateKey(KeyViewModel keyViewModel)
        {
            var keypos = keyViewModel.KeyPosition;
            var finger = GetFingerForKeyPosition(keypos);

            var mappings = _visualKeyboardLayout?.Map
                ?.Where(mapping => mapping.Key.KeyPosition.Equals(keypos))
                .OrderBy(mapping => (int)mapping.Key.Modifiers)
                .ToList();

            var defaultMapping = mappings?.FirstOrDefault().Value;
            var mappingTargetName = (defaultMapping is HardwareKeyMappingTarget mappedUnprintable)
                ? mappedUnprintable.HardwareKey.GetDefaultText()
                : defaultMapping?.Name;
            keyViewModel.Name = mappingTargetName ?? "";

            keyViewModel.Mappings = new Dictionary<IKeyboardKey, IMappingTarget>();
            mappings?.ForEach(mapping => keyViewModel.Mappings.Add(mapping));

            keyViewModel.HighlightedBackgroundBrush = new SolidColorBrush(GetColor(finger));
        }

        private Finger? GetFingerForKeyPosition(KeyPosition keyPosition)
        {
            return _fingerPositions.ContainsKey(keyPosition) ? (Finger?)_fingerPositions[keyPosition] : null;
        }

        private Color GetColor(Finger? finger)
        {
            return _fingerColors[finger];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyViewModel> GetEnumerator()
        {
            return GetAllKeys().GetEnumerator();
        }

        private IEnumerable<KeyViewModel> GetAllKeys()
        {
            foreach (var keyRowViewModel in Rows)
            {
                foreach (var keyViewModel in keyRowViewModel.Keys)
                {
                    yield return keyViewModel;
                }
            }
        }
    }
}