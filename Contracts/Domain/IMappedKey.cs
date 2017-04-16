using NTouchTypeTrainer.Contracts.Common;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IMappedKey : IStringExport
    {
        string Name { get; }
    }
}