using System;

namespace NTouchTypeTrainer.Domain.Enums
{
    [Flags]
    public enum Modifier
    {
        None = 0,

        Ctrl = 1,
        Shift = 2,
        AltGr = 4,
    }
}