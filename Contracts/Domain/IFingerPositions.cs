﻿using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Contracts.Domain
{
    public interface IFingerPositions
    {
        Finger this[HardwareKey key] { get; }

        bool ContainsKey(HardwareKey key);
    }
}
