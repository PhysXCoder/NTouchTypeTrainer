using NTouchTypeTrainer.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard;

namespace NTouchTypeTrainer.Domain.Keyboard
{
    /// <summary>
    /// Stores the width factor of each Key. 
    /// No factor (null in nullable float) means standard width = factor 1
    /// </summary>
    public class MechanicalKeyboardLayout : IMechanicalKeyboardLayout, IImmutable
    {
        private readonly Dictionary<KeyPosition, float?> _keySizefactorsDict;

        public int NumberOfRows { get; private set; }

        public int IndexMinRow { get; private set; }

        public int IndexMaxRow { get; private set; }

        public IReadOnlyDictionary<KeyPosition, float?> KeySizefactors
            => new ReadOnlyDictionary<KeyPosition, float?>(_keySizefactorsDict);

        public MechanicalKeyboardLayout(params IList<float?>[] keySizefactors)
        {
            _keySizefactorsDict = new Dictionary<KeyPosition, float?>();

            BuildKeySizesDict(keySizefactors);
        }

        public MechanicalKeyboardLayout(IReadOnlyDictionary<KeyPosition, float?> keySizefactorsDict)
        {
            _keySizefactorsDict = new Dictionary<KeyPosition, float?>();

            CopyKeySizesDict(keySizefactorsDict);
        }

        public int GetNumberOfKeysInRow(int iRow)
            => _keySizefactorsDict.Count(keyValuePair => (keyValuePair.Key.Row == iRow));

        public float? GetSizefactor(KeyPosition position)
        {
            var keyPosition = _keySizefactorsDict.Keys.First(keyPos => keyPos.Equals(position));
            return _keySizefactorsDict[keyPosition];
        }

        private void CopyKeySizesDict(IReadOnlyDictionary<KeyPosition, float?> keySizefactorsDict)
        {
            _keySizefactorsDict.Clear();

            foreach (var keyValuePair in keySizefactorsDict)
            {
                _keySizefactorsDict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            SetNumbers();
        }

        private void BuildKeySizesDict(IReadOnlyList<IList<float?>> keySizefactors)
        {
            _keySizefactorsDict.Clear();

            for (var iRow = 1; iRow <= keySizefactors.Count; iRow++)
            {
                var keySizesInRow = keySizefactors[iRow];

                for (var iKey = 1; iKey <= keySizesInRow.Count; iKey++)
                {
                    var size = keySizesInRow[iKey];
                    var pos = new KeyPosition(iRow, iKey);

                    _keySizefactorsDict.Add(pos, size);
                }
            }

            SetNumbers();
        }

        private void SetNumbers()
        {
            IndexMaxRow = _keySizefactorsDict.Keys.Max(keyPos => keyPos.Row);
            IndexMinRow = _keySizefactorsDict.Keys.Min(keyPos => keyPos.Row);
            NumberOfRows = IndexMaxRow - IndexMinRow + 1;
        }
    }
}