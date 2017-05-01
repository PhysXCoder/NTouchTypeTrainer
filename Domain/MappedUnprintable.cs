using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;

namespace NTouchTypeTrainer.Domain
{
    public class MappedUnprintable : IMappedKey, IStringImport<MappedUnprintable>, IStringImport<IMappedKey>, IEquatable<MappedUnprintable>, IImmutable
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

        public bool Equals(MappedUnprintable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return (Key == other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MappedUnprintable)obj);
        }

        public override int GetHashCode()
            => (int)Key;

        public override string ToString()
            => Name;

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
    }
}