using System;
using Caliburn.Micro;
using Eto.Drawing;
using NTouchTypeTrainer.Interfaces.Views;
using NTouchTypeTrainer.Messages;

namespace NTouchTypeTrainer.Views.Common
{
    public class DependentSizeGroup : IDependentSizeGroup, IHandle<SizeGroupChangedMsg>
    {
        private Size _size;

        private readonly ISizeGroup _sourceSizeGroup;
        private readonly Func<Size, Size> _dependencyFunc;
        private readonly IEventAggregator _eventAggregator;

        public Size Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    _eventAggregator.PublishOnCurrentThread(new SizeGroupChangedMsg() { Sender = this });
                }
            }
        }

        public DependentSizeGroup(
            ISizeGroup sourceSizeGroup,
            Func<Size, Size> dependencyFunc,
            IEventAggregator eventAggregator)
        {
            _sourceSizeGroup = sourceSizeGroup ?? throw new ArgumentNullException(nameof(sourceSizeGroup));
            _dependencyFunc = dependencyFunc ?? throw new ArgumentNullException(nameof(dependencyFunc));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            InitValues();

            _eventAggregator.Subscribe(this);
        }

        private void InitValues()
        {
            Handle(new SizeGroupChangedMsg() { Sender = _sourceSizeGroup });
        }

        public void Handle(SizeGroupChangedMsg message)
        {
            if (message?.Sender == _sourceSizeGroup)
            {
                // ReSharper disable once PossibleNullReferenceException
                Size = _dependencyFunc(_sourceSizeGroup.Size);
            }
        }
    }
}