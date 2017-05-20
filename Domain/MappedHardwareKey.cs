using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;

namespace NTouchTypeTrainer.Domain
{
    public class MappedHardwareKey :
        IMappingTarget,
        IStringImport<MappedHardwareKey>, IStringImport<IMappingTarget>,
        IEquatable<MappedHardwareKey>, IImmutable
    {
        public HardwareKey HardwareKey { get; }

        public string Name { get; }

        public Modifier Modifiers { get; }

        public MappedHardwareKey(HardwareKey hardwareKey, Modifier modifiers = Modifier.None)
        {
            HardwareKey = hardwareKey;
            Modifiers = modifiers;

            var modifiersString = modifiers.ToCombinedString();
            if (modifiersString != null)
            {
                Name = modifiersString + ModifierExtensions.ModifierCombiner;
            }

            Name += Enum.GetName(typeof(HardwareKey), hardwareKey);
        }

        bool IStringImport<MappedHardwareKey>.TryImport(string exportedString, out MappedHardwareKey outputMappedHardwareKey)
            => TryImport(exportedString, out outputMappedHardwareKey);

        IMappingTarget IStringImport<IMappingTarget>.Import(string exportedString)
            => Import(exportedString);

        MappedHardwareKey IStringImport<MappedHardwareKey>.Import(string exportedString)
            => Import(exportedString);

        public bool TryImport(string exportedString, out IMappingTarget outputMappedHardwareKey)
        {
            var parseSuccess = TryImport(exportedString, out MappedHardwareKey parseResult);

            outputMappedHardwareKey = parseResult;
            return parseSuccess;
        }

        public static MappedHardwareKey Import(string exportedString)
        {
            return (MappedHardwareKey)Enum.Parse(typeof(HardwareKey), exportedString, true);
        }

        public static bool TryImport(string exportedString, out MappedHardwareKey outputMappedHardwareKey)
        {
            var parseSuccess = Enum.TryParse(exportedString, true, out HardwareKey mappedKey);

            outputMappedHardwareKey = parseSuccess ? new MappedHardwareKey(mappedKey) : null;
            return parseSuccess;
        }

        public override string ToString()
            => Name;

        public bool Equals(MappedHardwareKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return HardwareKey == other.HardwareKey && Modifiers == other.Modifiers;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MappedHardwareKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)HardwareKey * 397) ^ (int)Modifiers;
            }
        }
    }
}