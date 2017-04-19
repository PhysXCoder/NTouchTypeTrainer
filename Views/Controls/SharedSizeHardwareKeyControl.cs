using Caliburn.Micro;
using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;

namespace NTouchTypeTrainer.Views.Controls
{
    public class SharedSizeHardwareKeyControl : HardwareKeyControl
    {
        protected ISharedSizeGroup SharedSizeGroup => SizeGroup as ISharedSizeGroup;

        protected SharedSizeHardwareKeyControl(IGraphicsProvider graphicsProvider)
            : base(graphicsProvider)
        { }

        public SharedSizeHardwareKeyControl(
            ISharedSizeGroup sharedSizeGroup,
            IEventAggregator eventAggregator,
            IGraphicsProvider graphicsProvider)
            : base(sharedSizeGroup, eventAggregator, graphicsProvider)
        { }

        protected override void UpdateSize()
        {
            var necessarySize = CalculateNecessarySize();

            if (SharedSizeGroup != null)
            {
                var height = SharedSizeGroup.RequestMinHeight(this, necessarySize.Height);
                var width = SharedSizeGroup.RequestMinWidth(this, necessarySize.Width);

                Size = new Size(width, height);
            }
            else
            {
                Size = necessarySize;
            }
        }
    }
}