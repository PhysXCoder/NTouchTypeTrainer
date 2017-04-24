using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;

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

        public static bool TryImport(string exportedString, out MappedUnprintable outputMappedUnprintable)
        {
            var parseSuccess = Enum.TryParse(exportedString, true, out HardwareKey mappedKey);

            outputMappedUnprintable = parseSuccess ? new MappedUnprintable(mappedKey) : null;
            return parseSuccess;
        }

        public static MappedUnprintable Import(string exportedString)
        {
            return (MappedUnprintable)Enum.Parse(typeof(HardwareKey), exportedString, true);
        }

        #region Interface implementations 

        MappedUnprintable IStringImport<MappedUnprintable>.Import(string exportedString) =>
            Import(exportedString);

        bool IStringImport<MappedUnprintable>.TryImport(string exportedString, out MappedUnprintable outputMappedUnprintable) =>
            TryImport(exportedString, out outputMappedUnprintable);

        IMappedKey IStringImport<IMappedKey>.Import(string exportedString) =>
            Import(exportedString);

        bool IStringImport<IMappedKey>.TryImport(string exportedString, out IMappedKey outputMappedKey)
        {
            var parseSuccess = TryImport(exportedString, out MappedUnprintable result);

            outputMappedKey = parseSuccess ? result : null;
            return parseSuccess;
        }

        #endregion
    }
}