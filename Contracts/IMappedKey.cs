using NTouchTypeTrainer.Contracts.Common;

namespace NTouchTypeTrainer.Contracts
{
	public interface IMappedKey : IStringExport
	{
		string Name { get; }
	}
}

