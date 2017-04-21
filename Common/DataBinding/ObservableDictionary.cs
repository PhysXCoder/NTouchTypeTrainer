using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NTouchTypeTrainer.Common.DataBinding
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private IDictionary<TKey, TValue> DictionaryInterface => _dictionary;
        private ICollection<KeyValuePair<TKey, TValue>> CollectionInterface => _dictionary;

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => DictionaryInterface.IsReadOnly;

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => Insert(key, value, false);
        }

        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool ContainsKey(TKey key)
            => _dictionary.ContainsKey(key);

        public bool Contains(KeyValuePair<TKey, TValue> item)
            => DictionaryInterface.Contains(item);

        public bool TryGetValue(TKey key, out TValue value)
            => _dictionary.TryGetValue(key, out value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => CollectionInterface.CopyTo(array, arrayIndex);

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var removeOldValue = _dictionary.TryGetValue(key, out TValue value);
            var removed = _dictionary.Remove(key);

            if (removeOldValue)
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
            }
            if (removed)
            {
                OnCollectionChanged();
            }

            return removed;
        }

        public void Clear()
        {
            if (_dictionary.Count > 0)
            {
                _dictionary.Clear();
                OnCollectionChanged();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void OnPropertyChanged()
        {
            OnPropertyChanged(nameof(_dictionary.Count));
            OnPropertyChanged("Item[]");
            OnPropertyChanged(nameof(_dictionary.Keys));
            OnPropertyChanged(nameof(_dictionary.Values));
        }

        private void OnCollectionChanged()
        {
            OnPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            OnPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            KeyValuePair<TKey, TValue> newItem,
            KeyValuePair<TKey, TValue> oldItem)
        {
            OnPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_dictionary.TryGetValue(key, out TValue oldItem))
            {
                if (add)
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
                if (!Equals(oldItem, value))
                {
                    _dictionary[key] = value;

                    OnCollectionChanged(NotifyCollectionChangedAction.Replace,
                        new KeyValuePair<TKey, TValue>(key, value),
                        new KeyValuePair<TKey, TValue>(key, oldItem));
                }
            }
            else
            {
                _dictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }
    }
}