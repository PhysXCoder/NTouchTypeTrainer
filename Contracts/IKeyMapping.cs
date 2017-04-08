using NTouchTypeTrainer.Contracts.Enums;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Contracts
{
	public interface IKeyMapping
	{
		HardwareKey PressedKey { get; }

		IReadOnlyDictionary<Modifier, IMappedKey> Mappings { get; }

		string Export(Modifier modifier);
	}
}

