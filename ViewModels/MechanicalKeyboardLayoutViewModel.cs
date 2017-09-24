using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NTouchTypeTrainer.ViewModels
{
    public class MechanicalKeyboardLayoutViewModel : BaseViewModel
    {
        private ObservableDictionary<int, ObservableCollection<int>> _keyIndexesInRow;
        private ObservableDictionary<KeyPosition, float?> _keySizes;

        public ObservableDictionary<int, ObservableCollection<int>> KeyIndexsInRow
        {
            get => _keyIndexesInRow;
            set
            {
                _keyIndexesInRow = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableDictionary<KeyPosition, float?> KeySizes
        {
            get => _keySizes;
            set
            {
                _keySizes = value;
                NotifyOfPropertyChange();
            }
        }

        public MechanicalKeyboardLayoutViewModel()
        {
            _keyIndexesInRow = new ObservableDictionary<int, ObservableCollection<int>>();
            _keySizes = new ObservableDictionary<KeyPosition, float?>();
        }

        public void LoadMechanicalLayout(IMechanicalKeyboardLayout layout)
        {
            var newSizes = new ObservableDictionary<KeyPosition, float?>();
            var newIndexes = new ObservableDictionary<int, ObservableCollection<int>>();

            for (var iRow = layout.IndexMinRow; iRow <= layout.IndexMaxRow; iRow++)
            {
                var keyIndexes = new List<int>();
                keyIndexes.AddRange(
                    layout.KeySizefactors.Keys
                        .Where(keyPos => keyPos.Row == iRow)
                        .Select(keyPos => keyPos.Index));

                var keyIndexesObservable = new ObservableCollection<int>();
                foreach (var iKey in keyIndexes)
                {
                    keyIndexesObservable.Add(iKey);

                    var keyPosition = new KeyPosition(iRow, iKey);
                    newSizes[keyPosition] = layout.GetSizefactor(keyPosition);
                }

                newIndexes.Add(iRow, keyIndexesObservable);
            }

            KeyIndexsInRow = newIndexes;
            KeySizes = newSizes;
        }
    }
}