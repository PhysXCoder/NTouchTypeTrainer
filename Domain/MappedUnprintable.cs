using System;
using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Enums;

namespace NTouchTypeTrainer.Domain
{
    public class MappedUnprintable : IMappedKey, IImmutable, IStringImport<MappedUnprintable>, IStringImport<IMappedKey>
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
            HardwareKey mappedKey;
            var parseSuccess = Enum.TryParse(exportedName, true, out mappedKey);

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
            MappedUnprintable result;
            var parseSuccess = TryImport(exportedName, out result);

            mappedOutputKey = parseSuccess ? result : null;
            return parseSuccess;
        }

        #endregion
    }
}