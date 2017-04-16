using System;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Domain
{
    public class MappedUnprintable : IMappedKey, IStringImport<MappedUnprintable>, IStringImport<IMappedKey>, IImmutable
    {
        public HardwareKey Key { get; }

        public string Name { get; }

        public MappedUnprintable(HardwareKey key)
        {
            Key = key;
            Name = Enum.GetName(typeof(HardwareKey), key)?.ToUpper();
        }

        public string Export() => Name;

        public static bool TryImport(string exportedName, out MappedUnprintable mappedOutputKey)
        {
            var parseSuccess = Enum.TryParse(exportedName, true, out HardwareKey mappedKey);

            mappedOutputKey = parseSuccess ? new MappedUnprintable(mappedKey) : null;
            return parseSuccess;
        }

        public static MappedUnprintable Import(string exportedName)
        {
            return (MappedUnprintable)Enum.Parse(typeof(HardwareKey), exportedName, true);
        }

        #region Interface implementations 

        MappedUnprintable IStringImport<MappedUnprintable>.Import(string exportedName) =>
            Import(exportedName);

        bool IStringImport<MappedUnprintable>.TryImport(string exportedName, out MappedUnprintable mappedOutputKey) =>
            TryImport(exportedName, out mappedOutputKey);

        IMappedKey IStringImport<IMappedKey>.Import(string exportedName) =>
            Import(exportedName);

        bool IStringImport<IMappedKey>.TryImport(string exportedName, out IMappedKey mappedOutputKey)
        {
            var parseSuccess = TryImport(exportedName, out MappedUnprintable result);

            mappedOutputKey = parseSuccess ? result : null;
            return parseSuccess;
        }

        #endregion
    }
}