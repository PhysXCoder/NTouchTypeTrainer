using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;

namespace NTouchTypeTrainer.Domain
{
    public class MappedCharacter :
        IMappingTarget,
        IStringImport<MappedCharacter>, IStringImport<IMappingTarget>,
        IEquatable<MappedCharacter>, IImmutable
    {
        public char Character { get; }

        public string Name { get; }

        public MappedCharacter(char character)
        {
            Character = character;
            Name = Character.ToString();
        }

        public static bool TryImport(string exportedString, out MappedCharacter outputMappedCharacter)
        {
            var parsingSuccess = char.TryParse(exportedString.Trim(), out char printableCharacter);

            outputMappedCharacter = parsingSuccess ? new MappedCharacter(printableCharacter) : null;
            return parsingSuccess;
        }

        public bool Equals(MappedCharacter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return (Character == other.Character);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MappedCharacter)obj);
        }

        public override int GetHashCode()
            => Character.GetHashCode();

        public override string ToString()
            => Name;

        public static MappedCharacter Import(string exportedString) =>
            new MappedCharacter(char.Parse(exportedString.Trim()));

        MappedCharacter IStringImport<MappedCharacter>.Import(string exportedString) =>
            Import(exportedString);

        bool IStringImport<MappedCharacter>.TryImport(string exportedString, out MappedCharacter outputMappedCharacter) =>
            TryImport(exportedString, out outputMappedCharacter);

        IMappingTarget IStringImport<IMappingTarget>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<IMappingTarget>.TryImport(string exportedString, out IMappingTarget outputMappedCharacter)
        {
            var parseSuccess = TryImport(exportedString, out MappedCharacter result);

            outputMappedCharacter = parseSuccess ? result : null;
            return parseSuccess;
        }
    }
}