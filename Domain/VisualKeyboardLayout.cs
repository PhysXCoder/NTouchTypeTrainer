using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NTouchTypeTrainer.Domain
{
    public class VisualKeyboardLayout : IVisualKeyboardLayout, IImmutable
    {
        private readonly Dictionary<PressedKey, IMappedKey> _keyMappings;
        private readonly Dictionary<IMappedKey, PressedKey> _reverseKeyMappings;

        public IReadOnlyDictionary<PressedKey, IMappedKey> KeyMappings
            => new ReadOnlyDictionary<PressedKey, IMappedKey>(_keyMappings);

        public IReadOnlyDictionary<IMappedKey, PressedKey> ReverseKeyMappings
            => new ReadOnlyDictionary<IMappedKey, PressedKey>(_reverseKeyMappings);

        public VisualKeyboardLayout(IEnumerable<IPressedKeyMapping> keyMappings)
            : this(keyMappings.ToDictionary(m => m.PressedKey, m => m.MappedKey))
        { }

        public VisualKeyboardLayout(IDictionary<PressedKey, IMappedKey> keyMappings)
        {
            _reverseKeyMappings = new Dictionary<IMappedKey, PressedKey>();
            _keyMappings = new Dictionary<PressedKey, IMappedKey>(keyMappings);

            foreach (var pressedKey in _keyMappings.Keys)
            {
                _reverseKeyMappings.Add(_keyMappings[pressedKey], pressedKey);
            }
        }
    }
}