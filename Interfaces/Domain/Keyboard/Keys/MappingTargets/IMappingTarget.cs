using System;

namespace NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets
{
    public interface IMappingTarget : IEquatable<IMappingTarget>
    {
        string Name { get; }
    }
}