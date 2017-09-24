using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;

namespace NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets
{
    /// <summary>
    /// Represents a character key mapping target (e.g. 'a', 'A', '1', 'ß', '\', ...)
    /// </summary>
    public class CharacterMappingTarget :
        IMappingTarget,
        IStringImport<CharacterMappingTarget>,
        IStringImport<IMappingTarget>,
        IEquatable<CharacterMappingTarget>,
        IImmutable
    {
        public char Character { get; }

        public string Name { get; }

        public CharacterMappingTarget(char character)
        {
            Character = character;
            Name = Character.ToString();
        }

        public static bool TryImport(string exportedString, out CharacterMappingTarget outputMappedCharacter)
        {
            var parsingSuccess = char.TryParse(exportedString.Trim(), out char printableCharacter);

            outputMappedCharacter = parsingSuccess ? new CharacterMappingTarget(printableCharacter) : null;
            return parsingSuccess;
        }

        public bool Equals(CharacterMappingTarget other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return (Character == other.Character);
        }

        public bool Equals(IMappingTarget other)
            => Equals(other as CharacterMappingTarget);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CharacterMappingTarget)obj);
        }

        public override int GetHashCode()
            => Character.GetHashCode();

        public override string ToString()
            => Name;

        public static CharacterMappingTarget Import(string exportedString) =>
            new CharacterMappingTarget(char.Parse(exportedString.Trim()));

        CharacterMappingTarget IStringImport<CharacterMappingTarget>.Import(string exportedString) =>
            Import(exportedString);

        bool IStringImport<CharacterMappingTarget>.TryImport(string exportedString, out CharacterMappingTarget outputMappedCharacter) =>
            TryImport(exportedString, out outputMappedCharacter);

        IMappingTarget IStringImport<IMappingTarget>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<IMappingTarget>.TryImport(string exportedString, out IMappingTarget outputMappedCharacter)
        {
            var parseSuccess = TryImport(exportedString, out CharacterMappingTarget result);

            outputMappedCharacter = parseSuccess ? result : null;
            return parseSuccess;
        }
    }
}