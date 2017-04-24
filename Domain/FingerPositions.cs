using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Serialization;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Domain
{
    public class FingerPositions : IFingerPositions, IImmutable
    {
        private readonly Dictionary<KeyPosition, Finger> _positionsDictionary;

        public FingerPositions(IDictionary<KeyPosition, Finger> positions)
        {
            _positionsDictionary = new Dictionary<KeyPosition, Finger>(positions);
        }

        public Finger this[KeyPosition keyPosition]
        {
            get
            {
                if (ContainsKey(keyPosition))
                {
                    return _positionsDictionary[keyPosition];
                }

                throw new KeyNotFoundException($"No finger is associated with position '{keyPosition}'!");
            }
        }

        public bool ContainsKey(KeyPosition keyPosition)
            => _positionsDictionary.ContainsKey(keyPosition);

        public IEnumerable<KeyValuePair<KeyPosition, Finger>> GetAllKeyFingerPairs()
            => _positionsDictionary;

        public bool TryImport(string exportedString, out FingerPositions outputFingerPositions)
            => FingerPositionsImporter.TryImport(exportedString, out outputFingerPositions);

        public FingerPositions Import(string exportedString)
            => FingerPositionsImporter.Import(exportedString);
    }
}