using System;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Serialization;
using NTouchTypeTrainer.Contracts.Domain;
using System.Collections.Generic;
using System.Linq;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Domain
{
    public class MechanicalKeyboardLayout : IMechanicalKeyboardLayout, IImmutable, IStringExport
    {
        private readonly IStringExporter<IMechanicalKeyboardLayout> _layoutExporter;

        public MechanicalKeyboardLayout(IEnumerable<IEnumerable<HardwareKey>> rows)
        {
            _layoutExporter = new MechanicalKeyboardLayoutExporter();

            KeyboardRows = rows
                ?.Select(keyRow => new List<HardwareKey>(keyRow).AsReadOnly())
                .ToList()
                .AsReadOnly()
                ?? throw new ArgumentNullException(nameof(rows));
        }

        public MechanicalKeyboardLayout(params IEnumerable<HardwareKey>[] rows)
            : this(rows?.ToList())
        { }

        public string Export() => _layoutExporter.Export(this);

        public IReadOnlyList<IReadOnlyList<HardwareKey>> KeyboardRows { get; }

        public IReadOnlyList<HardwareKey> this[int iRow]
        {
            get
            {
                if (0 <= iRow && iRow <= KeyboardRows.Count)
                {
                    return KeyboardRows[iRow];
                }
                else
                {
                    throw new IndexOutOfRangeException($"Can't access keyboard row with index {iRow}. "
                        + $"Only values between 0 and {KeyboardRows.Count} are possible!");
                }
            }
        }

        public HardwareKey this[int iRow, int iKey]
        {
            get
            {
                var row = this[iRow];

                if (0 <= iKey && iKey <= row.Count)
                {
                    return row[iKey];
                }
                else
                {
                    throw new IndexOutOfRangeException($"Can't access key with index {iKey} in row with index {iRow}. "
                        + $"Only values between 0 and {row.Count} are possible for key indexes!");
                }
            }
        }
    }
}