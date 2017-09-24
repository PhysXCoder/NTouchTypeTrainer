using Caliburn.Micro;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.Messages;
using System;
using System.Windows;

namespace NTouchTypeTrainer.Views.Common
{
    public class DependentSizeGroup : IDependentSizeGroup, IHandle<SizeGroupChangedMsg>
    {
        private Size _size;

        private readonly ISizeGroup _sourceSizeGroup;
        private readonly Func<Size, Size> _dependencyFunc;
        private readonly Func<Size, Size> _inverseDependencyFunc;
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
            Func<Size, Size> inverseDependencyFunc,
            IEventAggregator eventAggregator)
        {
            // ToDo: Check for null everywhere like this
            _sourceSizeGroup = sourceSizeGroup ?? throw new ArgumentNullException(nameof(sourceSizeGroup));
            _dependencyFunc = dependencyFunc ?? throw new ArgumentNullException(nameof(dependencyFunc));
            _inverseDependencyFunc = inverseDependencyFunc ?? throw new ArgumentNullException(nameof(inverseDependencyFunc));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            InitValues();

            // ToDo: Probably not necessary anymore when all objects are instantiated via Caliburn's IoC
            _eventAggregator.Subscribe(this);
        }

        private void InitValues()
        {
            ((IHandle<SizeGroupChangedMsg>)this).Handle(new SizeGroupChangedMsg() { Sender = _sourceSizeGroup });
        }

        void IHandle<SizeGroupChangedMsg>.Handle(SizeGroupChangedMsg message)
        {
            if (message?.Sender != null && message.Sender == _sourceSizeGroup)
            {
                Size = _dependencyFunc(_sourceSizeGroup.Size);
            }
        }

        public double RequestMinWidth(object requester, double necessaryWidth)
        {
            var necessarySourceSize = _inverseDependencyFunc(new Size(necessaryWidth, Size.Height));
            return _sourceSizeGroup.RequestMinWidth(requester, necessarySourceSize.Width);
        }

        public double RequestMinHeight(object requester, double necessaryHeight)
        {
            var necessarySourceSize = _inverseDependencyFunc(new Size(Size.Width, necessaryHeight));
            return _sourceSizeGroup.RequestMinHeight(requester, necessarySourceSize.Height);
        }

        public Size RequestMinSize(object requester, Size necessarySize)
        {
            var necessarySourceSize = _inverseDependencyFunc(necessarySize);
            return _sourceSizeGroup.RequestMinSize(requester, necessarySourceSize);
        }

        public void ClearRequests(object requester)
        {
            _sourceSizeGroup.ClearRequests(requester);
        }
    }
}
