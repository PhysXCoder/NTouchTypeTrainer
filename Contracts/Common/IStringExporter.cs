namespace NTouchTypeTrainer.Contracts.Common
{
    public interface IStringExporter<TData>
    {
        string Export(TData data);
    }
}