using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;

namespace NTouchTypeTrainer.Domain
{
    public class MappedCharacter : IMappedKey, IStringImport<MappedCharacter>, IStringImport<IMappedKey>, IImmutable
    {
        public char Character { get; }

        public string Name => Character.ToString();

        public MappedCharacter(char character)
        {
            Character = character;
        }

        public string Export() => Name;

        public static bool TryImport(string exportedString, out MappedCharacter outputMappedCharacter)
        {
            var parsingSuccess = char.TryParse(exportedString.Trim(), out char printableCharacter);

            outputMappedCharacter = parsingSuccess ? new MappedCharacter(printableCharacter) : null;
            return parsingSuccess;
        }

        public static MappedCharacter Import(string exportedString) =>
            new MappedCharacter(char.Parse(exportedString.Trim()));

        #region Interface implementations

        MappedCharacter IStringImport<MappedCharacter>.Import(string exportedString) =>
            Import(exportedString);

        bool IStringImport<MappedCharacter>.TryImport(string exportedString, out MappedCharacter outputMappedCharacter) =>
            TryImport(exportedString, out outputMappedCharacter);

        IMappedKey IStringImport<IMappedKey>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<IMappedKey>.TryImport(string exportedString, out IMappedKey outputMappedCharacter)
        {
            var parseSuccess = TryImport(exportedString, out MappedCharacter result);

            outputMappedCharacter = parseSuccess ? result : null;
            return parseSuccess;
        }

        #endregion
    }
}