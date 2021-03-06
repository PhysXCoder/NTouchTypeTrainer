﻿namespace NTouchTypeTrainer.Interfaces.Common
{
    public interface IStringImport<T>
    {
        bool TryImport(string exportedString, out T outputInstance);

        T Import(string exportedString);
    }
}