using System;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class HardwareKeyExtensions
    {
        public static IEnumerable<HardwareKey> GetAllKeys()
            => Enum.GetValues(typeof(HardwareKey)).Cast<HardwareKey>();

        private static readonly Dictionary<HardwareKey, string> SpecialDefaultTexts =
            new Dictionary<HardwareKey, string>
            {
                {HardwareKey.AccentGrave, "`"},
                {HardwareKey.D1, "1"},
                {HardwareKey.D2, "2"},
                {HardwareKey.D3, "3"},
                {HardwareKey.D4, "4"},
                {HardwareKey.D5, "5"},
                {HardwareKey.D6, "6"},
                {HardwareKey.D7, "7"},
                {HardwareKey.D8, "8"},
                {HardwareKey.D9, "9"},
                {HardwareKey.D0, "0"},
                {HardwareKey.Minus, "-"},
                {HardwareKey.Equal, "="},
                {HardwareKey.Backspace, "←"},

                {HardwareKey.Tab, (Environment.OSVersion.Platform == PlatformID.Unix)? "⭾" : "Tab"},
                {HardwareKey.BracketOpen, "["},
                {HardwareKey.BracketClose, "]"},
                {HardwareKey.Enter, (Environment.OSVersion.Platform == PlatformID.Unix)? "↲" : "˩"},

                {HardwareKey.CapsLock, "↓"},
                {HardwareKey.Semicolon, ";"},
                {HardwareKey.Apostrophe, "'"},
                {HardwareKey.NumberSign, "#"},

                {HardwareKey.ShiftLeft, (Environment.OSVersion.Platform == PlatformID.Unix)? "⇫" : "↑"},
                {HardwareKey.Backslash, "\\"},
                {HardwareKey.Comma, ","},
                {HardwareKey.Dot, "."},
                {HardwareKey.Slash, "/"},
                {HardwareKey.ShiftRight, (Environment.OSVersion.Platform == PlatformID.Unix)? "⇫" : "↑"},

                {HardwareKey.ControlLeft, "Ctrl"},
                {HardwareKey.SuperLeft, "❐"},
                {HardwareKey.Function, "Fn"},
                {HardwareKey.Alt, "Alt"},
                {HardwareKey.Space, " "},
                {HardwareKey.AltGr, "AltGr"},
                {HardwareKey.SuperRight, "❐"},
                {HardwareKey.Menu, "☰"},
                {HardwareKey.ControlRight, "Ctrl"},
            };

        public static string GetDefaultText(this HardwareKey key) =>
            SpecialDefaultTexts.ContainsKey(key) ? SpecialDefaultTexts[key] : key.ToString();

        public static bool IsModifier(this HardwareKey key)
            => ModifierMapping.ContainsKey(key);

        private static readonly Dictionary<HardwareKey, Modifier> ModifierMapping =
            new Dictionary<HardwareKey, Modifier>()
            {
                {HardwareKey.Alt, Modifier.Alt },
                {HardwareKey.AltGr, Modifier.AltGr },
                {HardwareKey.ShiftLeft, Modifier.Shift },
                {HardwareKey.ShiftRight, Modifier.Shift },
                {HardwareKey.ControlLeft, Modifier.Ctrl },
                {HardwareKey.ControlRight, Modifier.Ctrl },
            };
    }
}