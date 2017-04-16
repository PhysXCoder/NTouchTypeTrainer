using System;
using System.Runtime.CompilerServices;

namespace NTouchTypeTrainer.Common.Exceptions
{
    public class DeadlockException : SystemException
    {
        public DeadlockException([CallerMemberName] string callerName = null)
            : base($"Deadlock occurred while trying to get exclusive access in Method '{callerName}'!")
        { }
    }
}