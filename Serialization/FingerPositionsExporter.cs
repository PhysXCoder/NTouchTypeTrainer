using NTouchTypeTrainer.Common.Strings;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsExporter : FingerPositionsBasePorter, IStringExporter<IFingerPositions>
    {
        string IStringExporter<IFingerPositions>.Export(IFingerPositions data)
            => Export(data);

        public static string Export(IFingerPositions fingerPositions)
        {
            var builder = new StringBuilder();

            var keyPositionsForFinger = Enum.GetValues(typeof(Finger))
                .Cast<Finger>()
                .ToDictionary(finger => finger, finger => new List<KeyPosition>());

            foreach (var keyFingerPair in fingerPositions.GetAllKeyFingerPairs())
            {
                var finger = keyFingerPair.Value;
                var keyPosition = keyFingerPair.Key;

                keyPositionsForFinger[finger].Add(keyPosition);
            }

            foreach (var finger in keyPositionsForFinger.Keys)
            {
                builder
                    .Append(finger)
                    .Append(Separator).Append(KeySeparator);

                foreach (var keyPosition in keyPositionsForFinger[finger])
                {
                    builder
                        .Append(keyPosition.Row).Append(RowKeySeparator).Append(keyPosition.Key)
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