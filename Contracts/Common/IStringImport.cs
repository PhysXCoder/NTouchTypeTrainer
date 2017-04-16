namespace NTouchTypeTrainer.Contracts.Common
{
    public interface IStringImport<T>
    {
        bool TryImport(string exportedName, out T mappedOutputKey);

        T Import(string exportedName);
    }
}