using System;

namespace NTouchTypeTrainer.Domain.Enums
{
    [Flags]
    public enum Modifier
    {
        None = 0,
        Shift = 1,
        AltGr = 2,
        Ctrl = 4,

        All = None | Shift | AltGr | Ctrl
    }
}