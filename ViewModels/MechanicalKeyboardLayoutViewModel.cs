using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain;
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
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<KeyPosition, float?> KeySizes
        {
            get => _keySizes;
            set
            {
                _keySizes = value;
                OnPropertyChanged();
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
                    layout.KeySizes.Keys
                        .Where(keyPos => keyPos.Row == iRow)
                        .Select(keyPos => keyPos.Key));

                var keyIndexesObservable = new ObservableCollection<int>();
                foreach (var iKey in keyIndexes)
                {
                    keyIndexesObservable.Add(iKey);

                    var keyPosition = new KeyPosition(iRow, iKey);
                    newSizes[keyPosition] = layout.GetSize(keyPosition);
                }

                newIndexes.Add(iRow, keyIndexesObservable);
            }

            KeyIndexsInRow = newIndexes;
            KeySizes = newSizes;
        }
    }
}