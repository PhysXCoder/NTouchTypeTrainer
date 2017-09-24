using NTouchTypeTrainer.Interfaces.Domain.Keyboard.Keys.MappingTargets;

namespace NTouchTypeTrainer.Messages
{
    public class ExpectedKeyChangedMsg
    {
        public object Sender { get; set; }

        public IMappingTarget NewExpectedMappingTarget { get; set; }
    }
}