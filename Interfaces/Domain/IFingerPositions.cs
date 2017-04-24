﻿using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Interfaces.Domain
{
    public interface IFingerPositions
    {
        Finger this[KeyPosition key] { get; }

        bool ContainsKey(KeyPosition keyPosition);

        IEnumerable<KeyValuePair<KeyPosition, Finger>> GetAllKeyFingerPairs();
    }
}