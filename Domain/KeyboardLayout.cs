using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain
{
    public class KeyboardLayout : IKeyboardLayout, IImmutable, IStringExport
    {
        private readonly List<IKeyMapping> _digitsRow;
        private readonly List<IKeyMapping> _upperCharacterRow;
        private readonly List<IKeyMapping> _middleCharacterRow;
        private readonly List<IKeyMapping> _lowerCharacterRow;
        private readonly List<IKeyMapping> _controlKeyRow;

        private readonly List<IKeyMapping> _allRows;

        private readonly IKeyboardLayoutExporter _layoutExporter;

        public IReadOnlyList<IKeyMapping> DigitsRow => _digitsRow.AsReadOnly();
        public IReadOnlyList<IKeyMapping> UpperCharacterRow => _upperCharacterRow.AsReadOnly();
        public IReadOnlyList<IKeyMapping> MiddleCharacterRow => _middleCharacterRow.AsReadOnly();
        public IReadOnlyList<IKeyMapping> LowerCharacterRow => _lowerCharacterRow.AsReadOnly();
        public IReadOnlyList<IKeyMapping> ControlKeyRow => _controlKeyRow.AsReadOnly();

        public IReadOnlyList<IKeyMapping> AllRows => _allRows.AsReadOnly();

        public KeyboardLayout(
            IEnumerable<IKeyMapping> digitsRow,
            IEnumerable<IKeyMapping> upperCharacterRow,
            IEnumerable<IKeyMapping> middleCharacterRow,
            IEnumerable<IKeyMapping> lowerCharacterRow,
            IEnumerable<IKeyMapping> controlKeyRow)
        {
            _digitsRow = new List<IKeyMapping>(digitsRow);
            _upperCharacterRow = new List<IKeyMapping>(upperCharacterRow);
            _middleCharacterRow = new List<IKeyMapping>(middleCharacterRow);
            _lowerCharacterRow = new List<IKeyMapping>(lowerCharacterRow);
            _controlKeyRow = new List<IKeyMapping>(controlKeyRow);

            _allRows = _digitsRow
                .Concat(_upperCharacterRow)
                .Concat(_middleCharacterRow)
                .Concat(_lowerCharacterRow)
                .Concat(_controlKeyRow)
                .ToList();

            _layoutExporter = new KeyboardLayoutExporter();
        }

        public string Export() => _layoutExporter.Export(this);  
        
    }
}

