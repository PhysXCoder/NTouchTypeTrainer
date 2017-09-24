using System;

namespace NTouchTypeTrainer.Domain.Enums
{
    [Flags]
    public enum Modifier
    {
        None = 0,

        Shift = 1,
        Ctrl = 2,
        Alt = 4,
        AltGr = 8,
    }
}