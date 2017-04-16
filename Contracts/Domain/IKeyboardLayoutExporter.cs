namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IKeyboardLayoutExporter
    {
        string Export(IKeyboardLayout layout);
    }
}