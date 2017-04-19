using System.Collections.Generic;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class HardwareKeyExtensions
    {
        public static readonly List<HardwareKey> DigitsRow = new List<HardwareKey>()
        {
            HardwareKey.AccentGrave,
            HardwareKey.D1,
            HardwareKey.D2,
            HardwareKey.D3,
            HardwareKey.D4,
            HardwareKey.D5,
            HardwareKey.D6,
            HardwareKey.D7,
            HardwareKey.D8,
            HardwareKey.D9,
            HardwareKey.D0,
            HardwareKey.Minus,
            HardwareKey.Equal,
            HardwareKey.Backspace
        };

        public static readonly List<HardwareKey> UpperCharacterRow = new List<HardwareKey>()
        {
            HardwareKey.Tab,
            HardwareKey.Q,
            HardwareKey.W,
            HardwareKey.E,
            HardwareKey.R,
            HardwareKey.T,
            HardwareKey.Y,
            HardwareKey.U,
            HardwareKey.I,
            HardwareKey.O,
            HardwareKey.P,
            HardwareKey.BracketOpen,
            HardwareKey.BracketClose,
            HardwareKey.Enter
        };

        public static readonly List<HardwareKey> MiddleCharacterRow = new List<HardwareKey>()
        {
            HardwareKey.CapsLock,
            HardwareKey.A,
            HardwareKey.S,
            HardwareKey.D,
            HardwareKey.F,
            HardwareKey.G,
            HardwareKey.H,
            HardwareKey.J,
            HardwareKey.K,
            HardwareKey.L,
            HardwareKey.Semicolon,
            HardwareKey.Apostrophe,
            HardwareKey.NumberSign,
        };

        public static readonly List<HardwareKey> LowerCharacterRow = new List<HardwareKey>()
        {
            HardwareKey.ShiftLeft,
            HardwareKey.Backslash,
            HardwareKey.Z,
            HardwareKey.X,
            HardwareKey.C,
            HardwareKey.V,
            HardwareKey.B,
            HardwareKey.N,
            HardwareKey.M,
            HardwareKey.Comma,
            HardwareKey.Dot,
            HardwareKey.Slash,
            HardwareKey.ShiftRight,
        };

        public static readonly List<HardwareKey> ControlKeyRow = new List<HardwareKey>()
        {
            HardwareKey.ControlLeft,
            HardwareKey.SuperLeft,
            HardwareKey.Alt,
            HardwareKey.Space,
            HardwareKey.AltGr,
            HardwareKey.SuperRight,
            HardwareKey.Menu,
            HardwareKey.ControlRight
        };

        public static readonly List<List<HardwareKey>> AllRows = new List<List<HardwareKey>>()
        {
            DigitsRow,
            UpperCharacterRow,
            MiddleCharacterRow,
            LowerCharacterRow,
            ControlKeyRow
        };

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

                {HardwareKey.Tab, "⭾"},
                {HardwareKey.BracketOpen, "["},
                {HardwareKey.BracketClose, "]"},
                {HardwareKey.Enter, "↲"},

                {HardwareKey.CapsLock, "↓"},
                {HardwareKey.Semicolon, ";"},
                {HardwareKey.Apostrophe, "'"},
                {HardwareKey.NumberSign, "#"},

                {HardwareKey.ShiftLeft, "⇫"},
                {HardwareKey.Backslash, "\\"},
                {HardwareKey.Comma, ","},
                {HardwareKey.Dot, "."},
                {HardwareKey.Slash, "/"},
                {HardwareKey.ShiftRight, "⇫"},

                {HardwareKey.ControlLeft, "Ctrl"},
                {HardwareKey.SuperLeft, "❐"},
                {HardwareKey.Alt, "Alt"},
                {HardwareKey.Space, " "},
                {HardwareKey.AltGr, "AltGr"},
                {HardwareKey.SuperRight, "❐"},
                {HardwareKey.Menu, "☰"},
                {HardwareKey.ControlRight, "Ctrl"},
            };

        public static string GetDefaultText(this HardwareKey key) =>
            SpecialDefaultTexts.ContainsKey(key) ? SpecialDefaultTexts[key] : key.ToString();
    }
}