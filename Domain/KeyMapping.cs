using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NTouchTypeTrainer.Domain
{
    public class KeyMapping : IKeyMapping
    {
        public HardwareKey PressedKey { get; }
        public IReadOnlyDictionary<Modifier, IMappedKey> Mappings { get; }

        public const string Undefined = "UD";

        public KeyMapping(HardwareKey pressedKey, IMappedKey mappedTarget)
            : this(pressedKey, new Tuple<Modifier, IMappedKey>(Modifier.None, mappedTarget))
        { }

        public KeyMapping(HardwareKey pressedKey, params Tuple<Modifier, IMappedKey>[] mappedTargets)
            : this(pressedKey, mappedTargets?.Select(t => new KeyValuePair<Modifier, IMappedKey>(t.Item1, t.Item2)))
        { }

        public KeyMapping(HardwareKey pressedKey, IEnumerable<KeyValuePair<Modifier, IMappedKey>> mappedTargets)
        {
            PressedKey = pressedKey;

            var mappingDict = new Dictionary<Modifier, IMappedKey>();
            foreach (var mapTarget in mappedTargets)
            {
                mappingDict.Add(mapTarget.Key, mapTarget.Value);
            }

            Mappings = new ReadOnlyDictionary<Modifier, IMappedKey>(mappingDict);
        }

        public string Export(Modifier selectedModifier)
        {
            if (Mappings.ContainsKey(selectedModifier))
            {
                return Mappings[selectedModifier].Export();
            }
            else
            {
                return Undefined;
            }
        }
    }
}

