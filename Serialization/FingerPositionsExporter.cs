using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTouchTypeTrainer.Common.Strings;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsExporter : FingerPositionsBasePorter, IStringExporter<IFingerPositions>
    {
        string IStringExporter<IFingerPositions>.Export(IFingerPositions data)
            => Export(data);

        public static string Export(IFingerPositions fingerPositions)
        {
            var builder = new StringBuilder();

            var keysForFingerDictionary = Enum.GetValues(typeof(Finger))
                .Cast<Finger>()
                .ToDictionary(finger => finger, finger => new List<HardwareKey>());

            foreach (var keyFingerPair in fingerPositions.GetAllKeyFingerPairs())
            {
                var finger = keyFingerPair.Value;
                var key = keyFingerPair.Key;

                keysForFingerDictionary[finger].Add(key);
            }

            foreach (var finger in keysForFingerDictionary.Keys)
            {
                builder
                    .Append(finger)
                    .Append(Separator)
                    .Append(KeySeparator);

                foreach (var hardwareKey in keysForFingerDictionary[finger])
                {
                    builder
                        .Append(hardwareKey)
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