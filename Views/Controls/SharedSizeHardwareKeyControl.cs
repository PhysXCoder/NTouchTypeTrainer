using Caliburn.Micro;
using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Views.Controls
{
    public class SharedSizeHardwareKeyControl : HardwareKeyControl
    {
        protected ISharedSizeGroup SharedSizeGroup => SizeGroup as ISharedSizeGroup;

        protected SharedSizeHardwareKeyControl(HardwareKey key, IGraphicsProvider graphicsProvider)
            : base(key, graphicsProvider)
        { }

        public SharedSizeHardwareKeyControl(
            HardwareKey key,
            ISharedSizeGroup sharedSizeGroup,
            IEventAggregator eventAggregator,
            IGraphicsProvider graphicsProvider)
            : base(key, sharedSizeGroup, eventAggregator, graphicsProvider)
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