using System.Text;
using NTouchTypeTrainer.Common.Strings;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;
using static NTouchTypeTrainer.Domain.Enums.HardwareKeyExtensions;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsExporter : FingerPositionsBasePorter, IFingerPositionsExporter
    {
        public string Export(IFingerPositions fingerPositions)
        {
            var builder = new StringBuilder();

            foreach (var keyRow in AllRows)
            {
                foreach (var key in keyRow)
                {
                    var keyInRow = fingerPositions.ContainsKey(key);
                    builder
                        .Append((keyInRow ? (Finger?)fingerPositions[key] : null).Export())
                        .Append(KeySeparator);
                }
                builder.RemoveLast(KeySeparator);

                builder.Append(NewLine);
            }
            builder.RemoveLast(NewLine);

            return builder.ToString();
        }
    }
}