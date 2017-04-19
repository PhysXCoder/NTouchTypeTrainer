namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IFingerPositionsExporter
    {
        string Export(IFingerPositions fingerPositions);
    }
}