using Caliburn.Micro;
using Eto.Drawing;
using NTouchTypeTrainer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using NTouchTypeTrainer.Interfaces.Views;

namespace NTouchTypeTrainer.Views.Common
{
    public class SharedSizeGroup : ISharedSizeGroup
    {
        private int? _absoluteMinWidth;
        private int? _absoluteMaxWidth;
        private int? _absoluteMinHeight;
        private int? _absoluteMaxHeight;
        private Size _size;

        private readonly Dictionary<WeakReference, int> _requestedMinWidths;
        private readonly Dictionary<WeakReference, int> _requestedMinHeights;

        private readonly IEventAggregator _eventAggregator;

        public int? AbsoluteMinWidth
        {
            get => _absoluteMinWidth;
            set
            {
                if (_absoluteMinWidth != value)
                {
                    _absoluteMinWidth = value;
                    UpdateWidths();
                }
            }
        }

        public int? AbsoluteMaxWidth
        {
            get => _absoluteMaxWidth;
            set
            {
                if (_absoluteMaxWidth != value)
                {
                    _absoluteMaxWidth = value;
                    UpdateWidths();
                }
            }
        }

        public int? AbsoluteMinHeight
        {
            get => _absoluteMinHeight;
            set
            {
                if (_absoluteMinHeight != value)
                {
                    _absoluteMinHeight = value;
                    UpdateHeights();
                }
            }
        }

        public int? AbsoluteMaxHeight
        {
            get => _absoluteMaxHeight;
            set
            {
                if (_absoluteMaxHeight != value)
                {
                    _absoluteMaxHeight = value;
                    UpdateHeights();
                }
            }
        }

        public Size Size
        {
            get => _size;
            private set
            {
                if (value != _size)
                {
                    _size = value;

                    _eventAggregator.PublishOnCurrentThread(new SizeGroupChangedMsg() { Sender = this });
                }
            }
        }

        public string Name
        {
            get;
            set;
        }

        public SharedSizeGroup(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _requestedMinWidths = new Dictionary<WeakReference, int>();
            _requestedMinHeights = new Dictionary<WeakReference, int>();

            _size = new Size(1, 1);
        }

        public SharedSizeGroup(IEventAggregator eventAggregator, string name)
            : this(eventAggregator)
        {
            Name = name;
        }

        public int RequestMinWidth(object requester, int necessaryMinWidth)
        {
            RequestMinLength(requester, necessaryMinWidth, _requestedMinWidths);

            var minWidth = UpdateWidths();

            return minWidth;
        }

        public int RequestMinHeight(object requester, int necessaryMinHeight)
        {
            RequestMinLength(requester, necessaryMinHeight, _requestedMinHeights);
            return UpdateHeights();
        }

        public void ClearMinWidthRequest(object requester)
        {
            ClearMinLengthRequest(requester, _requestedMinWidths);
            UpdateWidths();
        }

        public void ClearMinHeightRequest(object requester)
        {
            ClearMinLengthRequest(requester, _requestedMinHeights);
            UpdateHeights();
        }

        public override string ToString()
        {
            var text = $"{nameof(SharedSizeGroup)}";

            if (!string.IsNullOrEmpty(Name))
            {
                text += "(" + Name + ")";
            }

            return text;
        }

        private void RequestMinLength(
            object requester,
            int necessaryMinLength,
            IDictionary<WeakReference, int> requestedMinLengths)
        {
            if (requester == null)
            {
                throw new ArgumentNullException(nameof(requester));
            }

            var weakRef = new WeakReference(requester);

            if (requestedMinLengths.ContainsKey(weakRef))
            {
                requestedMinLengths[weakRef] = necessaryMinLength;
            }
            else
            {
                requestedMinLengths.Add(weakRef, necessaryMinLength);
            }
        }

        private void ClearMinLengthRequest(object requester, IDictionary<WeakReference, int> requestedMinLengths)
        {
            if (requester == null)
            {
                throw new ArgumentNullException(nameof(requester));
            }

            var weakRef = new WeakReference(requester);
            if (requestedMinLengths.ContainsKey(weakRef))
            {
                requestedMinLengths.Remove(weakRef);
            }
        }

        private int UpdateWidths()
        {
            var minWidth = GetMinLength(_requestedMinWidths, AbsoluteMinWidth, AbsoluteMaxWidth);

            if (Size.Width != minWidth)
            {
                Size = new Size(minWidth, Size.Height);
            }

            return minWidth;
        }

        private int UpdateHeights()
        {
            var minHeight = GetMinLength(_requestedMinHeights, AbsoluteMinHeight, AbsoluteMaxHeight);

            if (Size.Height != minHeight)
            {
                Size = new Size(Size.Width, minHeight);
            }

            return minHeight;
        }

        private static int GetMinLength(
            IDictionary<WeakReference, int> requestedMinLengths,
            int? absoluteMinLength,
            int? absoluteMaxLength)
        {
            CleanRequests(requestedMinLengths);

            var minLength = absoluteMinLength ?? 0;
            if (requestedMinLengths.Count > 0)
            {
                minLength = Math.Max(minLength, requestedMinLengths.Max(entry => entry.Value));
            }

            if (absoluteMaxLength.HasValue)
            {
                minLength = Math.Min(minLength, absoluteMaxLength.Value);
            }

            return minLength;
        }

        private static void CleanRequests(IDictionary<WeakReference, int> requestsDictionary)
        {
            var entries = new List<WeakReference>(requestsDictionary.Keys);

            foreach (var weakRef in entries)
            {
                if (!weakRef.IsAlive)
                {
                    requestsDictionary.Remove(weakRef);
                }
            }
        }
    }
}