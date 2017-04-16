namespace NTouchTypeTrainer.Contracts.Views
{
    public interface ISharedSizeGroup : ISizeGroup
    {
        int? AbsoluteMinWidth { get; set; }

        int? AbsoluteMaxWidth { get; set; }

        int? AbsoluteMinHeight { get; set; }

        int? AbsoluteMaxHeight { get; set; }

        int RequestMinWidth(object requester, int necessaryMinWidth);

        int RequestMinHeight(object requester, int necessaryMinHeight);

        void ClearMinWidthRequest(object requester);

        void ClearMinHeightRequest(object requester);
    }
}