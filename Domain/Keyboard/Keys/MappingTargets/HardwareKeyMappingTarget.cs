using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;

namespace NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets
{
    /// <summary>
    /// Represents hardware key mapping targets (e.g. Ctrl, Shift, Alt, ...)
    /// </summary>
    public class HardwareKeyMappingTarget :
        IHardwareKeyMappingTarget,
        IStringImport<HardwareKeyMappingTarget>,
        IStringImport<IMappingTarget>,
        IEquatable<HardwareKeyMappingTarget>,
        IImmutable
    {
        public HardwareKey HardwareKey { get; }

        public string Name { get; }

        public Modifier Modifiers { get; }

        public HardwareKeyMappingTarget(HardwareKey hardwareKey, Modifier modifiers = Modifier.None)
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

        bool IStringImport<HardwareKeyMappingTarget>.TryImport(string exportedString, out HardwareKeyMappingTarget outputMappedHardwareKey)
            => TryImport(exportedString, out outputMappedHardwareKey);

        IMappingTarget IStringImport<IMappingTarget>.Import(string exportedString)
            => Import(exportedString);

        HardwareKeyMappingTarget IStringImport<HardwareKeyMappingTarget>.Import(string exportedString)
            => Import(exportedString);

        public bool TryImport(string exportedString, out IMappingTarget outputMappedHardwareKey)
        {
            var parseSuccess = TryImport(exportedString, out HardwareKeyMappingTarget parseResult);

            outputMappedHardwareKey = parseResult;
            return parseSuccess;
        }

        public static HardwareKeyMappingTarget Import(string exportedString)
        {
            return (HardwareKeyMappingTarget)Enum.Parse(typeof(HardwareKey), exportedString, true);
        }

        public static bool TryImport(string exportedString, out HardwareKeyMappingTarget outputMappedHardwareKey)
        {
            var parseSuccess = Enum.TryParse(exportedString, true, out HardwareKey mappedKey);

            outputMappedHardwareKey = parseSuccess ? new HardwareKeyMappingTarget(mappedKey) : null;
            return parseSuccess;
        }

        public override string ToString()
            => Name;

        public bool Equals(IMappingTarget other)
        {
            if (other is CharacterMappingTarget charMappingTarget)
            {
                return Equals(charMappingTarget);
            }

            return Equals(other as HardwareKeyMappingTarget);
        }

        public bool Equals(CharacterMappingTarget other)
        {
            var equals = false;

            // Try to convert char to key
            switch (Modifiers)
            {
                case Modifier.Shift:
                    equals = other.Character.ToString() == HardwareKey.ToString().ToUpper();
                    break;
                case Modifier.None:
                    equals = other.Character.ToString() == HardwareKey.ToString().ToLower();
                    break;
            }

            // Special case: Space
            if (!equals)
            {
                equals |= (other.Character == ' ' && HardwareKey == HardwareKey.Space);
            }

            return equals;
        }

        public bool Equals(HardwareKeyMappingTarget other)
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
            return Equals((HardwareKeyMappingTarget)obj);
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