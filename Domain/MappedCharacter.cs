using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Common;

namespace NTouchTypeTrainer.Domain
{
	public class MappedCharacter : IMappedKey, IStringImport<MappedCharacter>, IStringImport<IMappedKey>
    {
        public char Character { get; }

		public string Name => Character.ToString();

		public MappedCharacter(char character)
		{ 
			Character = character;
		}

		public string Export() => Name;

        public static bool TryImport(string exportedName, out MappedCharacter mappedOutputKey)
        {
            char printableCharacter;
            var parsingSuccess = char.TryParse(exportedName.Trim(), out printableCharacter);

            mappedOutputKey = parsingSuccess ? new MappedCharacter(printableCharacter) : null;
            return parsingSuccess;
        }

        public static MappedCharacter Import(string exportedName) => 
            new MappedCharacter(char.Parse(exportedName.Trim()));

        #region Interface implementations

        MappedCharacter IStringImport<MappedCharacter>.Import(string exportedName) =>
            Import(exportedName);

        bool IStringImport<MappedCharacter>.TryImport(string exportedName, out MappedCharacter mappedOutputKey) =>
            TryImport(exportedName, out mappedOutputKey);

        IMappedKey IStringImport<IMappedKey>.Import(string exportedName) 
            => Import(exportedName);

        bool IStringImport<IMappedKey>.TryImport(string exportedName, out IMappedKey mappedOutputKey)
        {
            MappedCharacter result;
            var parseSuccess = TryImport(exportedName, out result);

            mappedOutputKey = parseSuccess ? result : null;
            return parseSuccess;
        }

        #endregion
    }
}

