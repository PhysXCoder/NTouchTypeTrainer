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

        private readonly IFingerPositionsExporter _exporter;
        private readonly IFingerPositionsImporter _importer;

        public FingerPositions(IDictionary<HardwareKey, Finger> positions)
        {
            _positionsDictionary = new Dictionary<HardwareKey, Finger>(positions);

            _exporter = new FingerPositionsExporter();
            _importer = new FingerPositionsImporter();
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

        public bool ContainsKey(HardwareKey key) => _positionsDictionary.ContainsKey(key);

        public bool TryImport(string exportedString, out FingerPositions outputFingerPositions) =>
            _importer.TryImport(exportedString, out outputFingerPositions);

        public FingerPositions Import(string exportedString) => _importer.Import(exportedString);

        public string Export() => _exporter.Export(this);
    }
}