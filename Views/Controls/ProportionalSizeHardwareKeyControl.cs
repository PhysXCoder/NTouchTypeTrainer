using System;
using Caliburn.Micro;
using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Views.Common;

namespace NTouchTypeTrainer.Views.Controls
{
    public class ProportionalSizeHardwareKeyControl : HardwareKeyControl
    {
        protected IDependentSizeGroup DependentSizeGroup => SizeGroup as IDependentSizeGroup;

        public ProportionalSizeHardwareKeyControl(
            HardwareKey key,
            float factor,
            ISizeGroup sourceSizeGroup,
            IEventAggregator eventAggregator,
            IGraphicsProvider graphicsProvider)
            : base(
                  key,
                  CreateDependetSizeGroup(factor, sourceSizeGroup, eventAggregator),
                  eventAggregator,
                  graphicsProvider)
        { }

        private static ISizeGroup CreateDependetSizeGroup(
            float factor,
            ISizeGroup sourceSizeGroup,
            IEventAggregator eventAggregator)
        {
            return new DependentSizeGroup(
                sourceSizeGroup,
                size => new Size(
                    (int)Math.Ceiling(size.Width * factor),
                    size.Height),
                eventAggregator);
        }

        protected override void UpdateSize()
        {
            Size = DependentSizeGroup?.Size ?? CalculateNecessarySize();
        }
    }
}