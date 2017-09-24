using NTouchTypeTrainer.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard;

namespace NTouchTypeTrainer.Domain.Keyboard
{
    /// <summary>
    /// Stores the mappings Key -> MappingTarget (and the reverse mapping).
    /// 
    /// The mappings can be accessed via the Map and ReversMap dictionaries, 
    /// or using the Mappings list.
    /// </summary>
    public class VisualKeyboardLayout : IVisualKeyboardLayout, IImmutable
    {
        private readonly List<IKeyboardKeyMapping> _mappings;
        private readonly Dictionary<IKeyboardKey, IMappingTarget> _map;
        private readonly Dictionary<IMappingTarget, IKeyboardKey> _reverseMap;

        public IReadOnlyList<IKeyboardKeyMapping> Mappings
            => new ReadOnlyCollection<IKeyboardKeyMapping>(_mappings);

        public IReadOnlyDictionary<IKeyboardKey, IMappingTarget> Map
            => new ReadOnlyDictionary<IKeyboardKey, IMappingTarget>(_map);

        public IReadOnlyDictionary<IMappingTarget, IKeyboardKey> ReverseMap
            => new ReadOnlyDictionary<IMappingTarget, IKeyboardKey>(_reverseMap);

        public VisualKeyboardLayout(IEnumerable<IKeyboardKeyMapping> keyMappings)
            : this(keyMappings.ToDictionary(m => m.KeyboardKey, m => m.MappingTarget))
        { }

        public VisualKeyboardLayout(IDictionary<IKeyboardKey, IMappingTarget> map)
        {
            _map = new Dictionary<IKeyboardKey, IMappingTarget>(map);
            _reverseMap = new Dictionary<IMappingTarget, IKeyboardKey>();
            _mappings = new List<IKeyboardKeyMapping>();

            foreach (var keyboardKey in _map.Keys)
            {
                _mappings.Add(new KeyMapping(keyboardKey, _map[keyboardKey]));

                try
                {
                    _reverseMap.Add(_map[keyboardKey], keyboardKey);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(
                        $"Error adding reverse mapping {keyboardKey} -> '{_map[keyboardKey]}'. "
                            + "Perhaps it is defined multiple times?",
                        ex);
                }
            }
        }
    }
}