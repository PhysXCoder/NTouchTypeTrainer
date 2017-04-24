using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NTouchTypeTrainer.Domain
{
    public class MechanicalKeyboardLayout : IMechanicalKeyboardLayout, IImmutable
    {
        private readonly Dictionary<KeyPosition, float?> _keySizesDict;

        public int NumberOfRows { get; private set; }

        public int IndexMinRow { get; private set; }

        public int IndexMaxRow { get; private set; }

        public IReadOnlyDictionary<KeyPosition, float?> KeySizes
            => new ReadOnlyDictionary<KeyPosition, float?>(_keySizesDict);

        public MechanicalKeyboardLayout(params IList<float?>[] keySizes)
        {
            _keySizesDict = new Dictionary<KeyPosition, float?>();

            BuildKeySizesDict(keySizes);
        }

        public MechanicalKeyboardLayout(IReadOnlyDictionary<KeyPosition, float?> keySizesDict)
        {
            _keySizesDict = new Dictionary<KeyPosition, float?>();

            CopyKeySizesDict(keySizesDict);
        }

        public int GetNumberOfKeysInRow(int iRow)
            => _keySizesDict.Count(keyValuePair => (keyValuePair.Key.Row == iRow));

        public float? GetSize(KeyPosition position)
        {
            var keyPosition = _keySizesDict.Keys.First(keyPos => keyPos.Equals(position));
            return _keySizesDict[keyPosition];
        }

        private void CopyKeySizesDict(IReadOnlyDictionary<KeyPosition, float?> keySizesDict)
        {
            _keySizesDict.Clear();

            foreach (var keyValuePair in keySizesDict)
            {
                _keySizesDict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            SetNumbers();
        }

        private void BuildKeySizesDict(IReadOnlyList<IList<float?>> keySizes)
        {
            _keySizesDict.Clear();

            for (var iRow = 1; iRow <= keySizes.Count; iRow++)
            {
                var keySizesInRow = keySizes[iRow];

                for (var iKey = 1; iKey <= keySizesInRow.Count; iKey++)
                {
                    var size = keySizesInRow[iKey];
                    var pos = new KeyPosition(iRow, iKey);

                    _keySizesDict.Add(pos, size);
                }
            }

            SetNumbers();
        }

        private void SetNumbers()
        {
            IndexMaxRow = _keySizesDict.Keys.Max(keyPos => keyPos.Row);
            IndexMinRow = _keySizesDict.Keys.Min(keyPos => keyPos.Row);
            NumberOfRows = IndexMaxRow - IndexMinRow + 1;
        }
    }
}