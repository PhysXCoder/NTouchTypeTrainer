using NTouchTypeTrainer.Interfaces.Views;

namespace NTouchTypeTrainer.Messages
{
    public class SizeGroupChangedMsg
    {
        public ISizeGroup Sender { get; set; }

        public override string ToString() => $"{nameof(SizeGroupChangedMsg)}: {nameof(Sender)}={Sender}";
    }
}