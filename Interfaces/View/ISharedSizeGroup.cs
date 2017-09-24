namespace NTouchTypeTrainer.Interfaces.View
{
    public interface ISharedSizeGroup : ISizeGroup
    {
        double? AbsoluteMinWidth { get; set; }

        double? AbsoluteMaxWidth { get; set; }

        double? AbsoluteMinHeight { get; set; }

        double? AbsoluteMaxHeight { get; set; }

        void ClearMinWidthRequest(object requester);

        void ClearMinHeightRequest(object requester);
    }
}