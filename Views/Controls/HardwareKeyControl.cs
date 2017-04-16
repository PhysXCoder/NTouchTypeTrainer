using System.Diagnostics.CodeAnalysis;
using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NLog;
using NTouchTypeTrainer.Common.GuiExtensions;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Messages;
using static NTouchTypeTrainer.Common.Logging.LoggingExtensions;
using static NTouchTypeTrainer.Common.Strings.ToStringConverterHelper;

namespace NTouchTypeTrainer.Views.Controls
{
    public abstract class HardwareKeyControl : Button, IHandle<SizeGroupChangedMsg>
    {
        protected readonly ILogger Logger;
        protected readonly ISizeGroup SizeGroup;
        protected readonly IGraphicsProvider GraphicsProvider;

        protected readonly Size PaddingSize = new Size(8, 5);

        public HardwareKey Key
        {
            get;
            set;
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                UpdateSize();
            }
        }

        private HardwareKeyControl(IGraphicsProvider graphicsProvider)
        {
            GraphicsProvider = graphicsProvider;
            Logger = NLog.LogManager.GetCurrentClassLogger();

            InitValues();
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        protected HardwareKeyControl(HardwareKey key, IGraphicsProvider graphicsProvider)
            : this(graphicsProvider)
        {
            Key = key;
            Text = key.GetDefaultText();
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        protected HardwareKeyControl(
            HardwareKey key,
            ISizeGroup sizeGroup,
            IEventAggregator eventAggregator,
            IGraphicsProvider graphicsProvider)
            : this(key, graphicsProvider)
        {
            SizeGroup = sizeGroup;

            Text = key.GetDefaultText();

            eventAggregator.Subscribe(this);
        }

        private void InitValues()
        {
            Enabled = false;
            Size = new Size(1, 1);
            TextColor = SystemColors.ControlText;
        }

        public void Handle(SizeGroupChangedMsg message)
        {
            if (message.Sender == SizeGroup)
            {
                Logger.Info(MsgReceived(message, Text));

                UpdateSize();
            }
        }

        public override string ToString() => GetObjectId<HardwareKeyControl>(Text);

        protected abstract void UpdateSize();

        protected Size CalculateNecessarySize()
        {
            return (Size)GraphicsProvider.Graphics.MeasureString(Font, Text).Inflate(PaddingSize * 2);
        }
    }
}