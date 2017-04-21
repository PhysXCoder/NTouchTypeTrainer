using System.Collections.Generic;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Serialization;

namespace NTouchTypeTrainer.Domain
{
    public class FingerPositions : IImmutable, IFingerPositions, IStringExport, IStringImport<FingerPositions>
    {
        private readonly Dictionary<HardwareKey, Finger> _positionsDictionary;

        public FingerPositions(IDictionary<HardwareKey, Finger> positions)
        {
            _positionsDictionary = new Dictionary<HardwareKey, Finger>(positions);
        }

        public Finger this[HardwareKey key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return _positionsDictionary[key];
                }

                throw new KeyNotFoundException($"No finger is associated with key '{key}'!");
            }
        }

        public bool ContainsKey(HardwareKey key)
            => _positionsDictionary.ContainsKey(key);

        public IEnumerable<KeyValuePair<HardwareKey, Finger>> GetAllKeyFingerPairs()
            => _positionsDictionary;

        public bool TryImport(string exportedString, out FingerPositions outputFingerPositions)
            => FingerPositionsImporter.TryImport(exportedString, out outputFingerPositions);

        public FingerPositions Import(string exportedString)
            => FingerPositionsImporter.Import(exportedString);

        public string Export()
            => FingerPositionsExporter.Export(this);
    }
}