using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NTouchTypeTrainer.Domain
{
    public class VisualKeyboardLayout : IVisualKeyboardLayout, IImmutable
    {
        private readonly Dictionary<IKeyboardKey, IMappingTarget> _keyMappings;
        private readonly Dictionary<IMappingTarget, IKeyboardKey> _reverseKeyMappings;

        public IReadOnlyDictionary<IKeyboardKey, IMappingTarget> KeyMappings
            => new ReadOnlyDictionary<IKeyboardKey, IMappingTarget>(_keyMappings);

        public IReadOnlyDictionary<IMappingTarget, IKeyboardKey> ReverseKeyMappings
            => new ReadOnlyDictionary<IMappingTarget, IKeyboardKey>(_reverseKeyMappings);

        public VisualKeyboardLayout(IEnumerable<IKeyboardKeyMapping> keyMappings)
            : this(keyMappings.ToDictionary(m => m.KeyboardKey, m => m.MappedKey))
        { }

        public VisualKeyboardLayout(IDictionary<IKeyboardKey, IMappingTarget> keyMappings)
        {
            _reverseKeyMappings = new Dictionary<IMappingTarget, IKeyboardKey>();
            _keyMappings = new Dictionary<IKeyboardKey, IMappingTarget>(keyMappings);

            foreach (var keyboardKey in _keyMappings.Keys)
            {
                try
                {
                    _reverseKeyMappings.Add(_keyMappings[keyboardKey], keyboardKey);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(
                        $"Error adding reverse mapping {keyboardKey} -> '{_keyMappings[keyboardKey]}'. "
                            + "Perhaps it is defined multiple times?",
                        ex);
                }
            }
        }
    }
}