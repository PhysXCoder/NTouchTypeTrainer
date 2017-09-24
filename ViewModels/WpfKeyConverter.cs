using NTouchTypeTrainer.Common.Primitives;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NTouchTypeTrainer.ViewModels
{
    public class WpfKeyConverter
    {
        public static IHardwareKeyMappingTarget ConvertPressedKey(KeyEventArgs keyEventArgs, KeyboardDevice keyboardDevice)
        {
            var pressedKey = keyEventArgs.Key; // To be more precise, that's the key that has been pressed and is now released
            var otherKeysPressed = EnumExtensions.GetValues<Key>()
                .Where(key => key != Key.None)
                .Where(key => (keyboardDevice.GetKeyStates(key) & KeyStates.Down) != 0)
                .ToList();

            var modifierKeys = otherKeysPressed.Where(ModifierKeys.ContainsKey).ToList();

            // Only valid if key is known!
            var valid = KeyMapping.ContainsKey(pressedKey);
            if (!valid) return null;

            // Ignore other regular keys, but take into account modifiers
            var modifiers = modifierKeys
                .Select(key => ModifierKeys[key])
                .Aggregate(Modifier.None, (mod, acc) => mod | acc);
            var pressedConvertedKey = new HardwareKeyMappingTarget(KeyMapping[pressedKey], modifiers);

            return pressedConvertedKey;
        }

        private static readonly Dictionary<Key, Modifier> ModifierKeys = new Dictionary<Key, Modifier>()
        {
            {Key.LeftCtrl, Modifier.Ctrl},
            {Key.RightCtrl, Modifier.Ctrl},
            {Key.LeftShift, Modifier.Shift},
            {Key.RightShift, Modifier.Shift},
            {Key.LeftAlt, Modifier.Alt},
            {Key.RightAlt, Modifier.AltGr},
        };

        private static readonly Dictionary<Key, HardwareKey> KeyMapping = new Dictionary<Key, HardwareKey>()
        {
            {Key.None, HardwareKey.None},

            {Key.Oem5, HardwareKey.AccentGrave},        // German keyboard: ^
            {Key.D1, HardwareKey.D1},
            {Key.D2, HardwareKey.D2},
            {Key.D3, HardwareKey.D3},
            {Key.D4, HardwareKey.D4},
            {Key.D5, HardwareKey.D5},
            {Key.D6, HardwareKey.D6},
            {Key.D7, HardwareKey.D7},
            {Key.D8, HardwareKey.D8},
            {Key.D9, HardwareKey.D9},
            {Key.D0, HardwareKey.D0},
            {Key.OemOpenBrackets, HardwareKey.Minus},   // German keyboard: ß
            {Key.Oem6, HardwareKey.Equal},              // German keyboard: `
            {Key.Back, HardwareKey.Backspace},

            {Key.A, HardwareKey.A},
            {Key.B, HardwareKey.B},
            {Key.C, HardwareKey.C},
            {Key.D, HardwareKey.D},
            {Key.E, HardwareKey.E},
            {Key.F, HardwareKey.F},
            {Key.G, HardwareKey.G},
            {Key.H, HardwareKey.H},
            {Key.I, HardwareKey.I},
            {Key.J, HardwareKey.J},
            {Key.K, HardwareKey.K},
            {Key.L, HardwareKey.L},
            {Key.M, HardwareKey.M},
            {Key.N, HardwareKey.N},
            {Key.O, HardwareKey.O},
            {Key.P, HardwareKey.P},
            {Key.Q, HardwareKey.Q},
            {Key.R, HardwareKey.R},
            {Key.S, HardwareKey.S},
            {Key.T, HardwareKey.T},
            {Key.U, HardwareKey.U},
            {Key.V, HardwareKey.V},
            {Key.W, HardwareKey.W},
            {Key.X, HardwareKey.X},
            {Key.Y, HardwareKey.Y},
            {Key.Z, HardwareKey.Z},

            {Key.Tab, HardwareKey.Tab},
            {Key.Oem1, HardwareKey.BracketOpen},        // German keyboard: ü
            {Key.OemPlus, HardwareKey.BracketClose},    // German keyboard: +
            {Key.Return, HardwareKey.Enter},

            {Key.CapsLock, HardwareKey.CapsLock},
            {Key.Oem3, HardwareKey.Semicolon},          // German keyboard: ö
            {Key.OemQuotes, HardwareKey.Apostrophe},    // German keyboard: ä
            {Key.OemQuestion, HardwareKey.NumberSign},  // German keyboard: #

            {Key.LeftShift, HardwareKey.ShiftLeft},
            {Key.OemBackslash, HardwareKey.Backslash},  // German keyboard: <
            {Key.OemComma, HardwareKey.Comma},          // German keyboard: ,
            {Key.OemPeriod, HardwareKey.Dot},           // German keyboard: ,
            {Key.OemMinus, HardwareKey.Slash},          // German keyboard: -
            {Key.RightShift, HardwareKey.ShiftRight},

            {Key.LeftCtrl, HardwareKey.ControlLeft},
            {Key.LWin, HardwareKey.SuperLeft},
            {Key.LeftAlt, HardwareKey.Alt},
            {Key.Space, HardwareKey.Space},
            {Key.RightAlt, HardwareKey.AltGr},
            {Key.RWin, HardwareKey.SuperRight},
            {Key.Apps, HardwareKey.Menu},
            {Key.RightCtrl, HardwareKey.ControlRight},
        };

    }
}