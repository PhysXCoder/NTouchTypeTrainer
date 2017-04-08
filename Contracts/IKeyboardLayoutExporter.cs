namespace NTouchTypeTrainer.Contracts
{
    public interface IKeyboardLayoutExporter
    {
        string Export(IKeyboardLayout layout);
    }
}