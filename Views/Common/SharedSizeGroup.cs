using Caliburn.Micro;
using NTouchTypeTrainer.Common.Gui;
using NTouchTypeTrainer.Common.Primitives;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NTouchTypeTrainer.Views.Common
{
    public class SharedSizeGroup : ISharedSizeGroup
    {
        private double? _absoluteMinWidth;
        private double? _absoluteMaxWidth;
        private double? _absoluteMinHeight;
        private double? _absoluteMaxHeight;
        private Size _size;

        private readonly Dictionary<WeakReference, double> _requestedMinWidths;
        private readonly Dictionary<WeakReference, double> _requestedMinHeights;

        private readonly IEventAggregator _eventAggregator;

        public double? AbsoluteMinWidth
        {
            get => _absoluteMinWidth;
            set
            {
                if (!_absoluteMinWidth.IsApproximatelyEqual(value))
                {
                    _absoluteMinWidth = value;
                    UpdateWidths();
                }
            }
        }

        public double? AbsoluteMaxWidth
        {
            get => _absoluteMaxWidth;
            set
            {
                if (!_absoluteMaxWidth.IsApproximatelyEqual(value))
                {
                    _absoluteMaxWidth = value;
                    UpdateWidths();
                }
            }
        }

        public double? AbsoluteMinHeight
        {
            get => _absoluteMinHeight;
            set
            {
                if (!_absoluteMinHeight.IsApproximatelyEqual(value))
                {
                    _absoluteMinHeight = value;
                    UpdateHeights();
                }
            }
        }

        public double? AbsoluteMaxHeight
        {
            get => _absoluteMaxHeight;
            set
            {
                if (!_absoluteMaxHeight.IsApproximatelyEqual(value))
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
                if (!_size.IsApproximatelyEqual(value))
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

            _requestedMinWidths = new Dictionary<WeakReference, double>();
            _requestedMinHeights = new Dictionary<WeakReference, double>();

            _size = new Size(1, 1);
        }

        public SharedSizeGroup(IEventAggregator eventAggregator, string name)
            : this(eventAggregator)
        {
            Name = name;
        }

        public double RequestMinWidth(object requester, double necessaryWidth)
        {
            RequestMinLength(requester, necessaryWidth, _requestedMinWidths);
            return UpdateWidths();
        }

        public double RequestMinHeight(object requester, double necessaryHeight)
        {
            RequestMinLength(requester, necessaryHeight, _requestedMinHeights);
            return UpdateHeights();
        }

        public Size RequestMinSize(object requester, Size necessarySize)
        {
            RequestMinLength(requester, necessarySize.Width, _requestedMinWidths);
            RequestMinLength(requester, necessarySize.Height, _requestedMinHeights);
            return new Size(UpdateWidths(), UpdateHeights());
        }

        public void ClearRequests(object requester)
        {
            ClearMinHeightRequest(requester);
            ClearMinWidthRequest(requester);
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
            double necessaryMinLength,
            IDictionary<WeakReference, double> requestedMinLengths)
        {
            if (requester == null)
            {
                throw new ArgumentNullException(nameof(requester));
            }

            var weakRef = requestedMinLengths.Keys.FirstOrDefault(wref => wref.IsAlive && wref.Target == requester);
            var weakRefExisted = (weakRef != null);
            if (!weakRefExisted)
            {
                weakRef = new WeakReference(requester);
                requestedMinLengths.Add(weakRef, necessaryMinLength);
            }
            else
            {
                requestedMinLengths[weakRef] = necessaryMinLength;
            }
        }

        private void ClearMinLengthRequest(object requester, IDictionary<WeakReference, double> requestedMinLengths)
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

        private double UpdateWidths()
        {
            var minWidth = GetMinLength(_requestedMinWidths, AbsoluteMinWidth, AbsoluteMaxWidth);

            var different = !Size.Width.IsApproximatelyEqual(minWidth);
            if (different)
            {
                Size = new Size(minWidth, Size.Height);
            }

            return minWidth;
        }

        private double UpdateHeights()
        {
            var minHeight = GetMinLength(_requestedMinHeights, AbsoluteMinHeight, AbsoluteMaxHeight);

            if (!Size.Height.IsApproximatelyEqual(minHeight))
            {
                Size = new Size(Size.Width, minHeight);
            }

            return minHeight;
        }

        private static double GetMinLength(
            IDictionary<WeakReference, double> requestedMinLengths,
            double? absoluteMinLength,
            double? absoluteMaxLength)
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

        private static void CleanRequests(IDictionary<WeakReference, double> requestsDictionary)
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