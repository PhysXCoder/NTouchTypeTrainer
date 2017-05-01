using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using System;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsImporter : BaseImporter, IStringImport<FingerPositions>
    {
        FingerPositions IStringImport<FingerPositions>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<FingerPositions>.TryImport(string exportedString, out FingerPositions outputInstance)
            => TryImport(exportedString, out outputInstance);

        public static FingerPositions Import(string exportedString)
        {
            var parseSuccess = TryImport(exportedString, out FingerPositions fingerPos);

            if (parseSuccess)
            {
                return fingerPos;
            }
            else
            {
                throw new FormatException($"Couldn't parse finger positions '{exportedString}'!");
            }
        }

        public static bool TryImport(string exportedString, out FingerPositions outputInstance)
            => Import(exportedString, false, out outputInstance);

        private static bool Import(string exportedString, bool throwExceptions, out FingerPositions outputInstance)
        {
            outputInstance = null;
            var positionsDict = new Dictionary<KeyPosition, Finger>();

            int iRow = 0;
            var lines = exportedString.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                ++iRow;

                int iKey = 0;
                var fingerStrings = line.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var fingerString in fingerStrings)
                {
                    iKey++;

                    if (!GetFinger(throwExceptions, fingerString, out Finger finger))
                    {
                        return false;
                    }

                    var pos = new KeyPosition(iRow, iKey);
                    positionsDict.Add(pos, finger);
                }
            }


            outputInstance = new FingerPositions(positionsDict);
            return true;
        }

        private static bool GetFinger(bool throwExceptions, string fingerToken, out Finger finger)
        {
            if (!throwExceptions)
            {
                var parseSuccess = FingerExtensions.TryImport(fingerToken, out Finger? parsedFinger);
                if (parseSuccess && (parsedFinger != null))
                {
                    finger = parsedFinger.Value;
                    return true;
                }

                finger = Finger.MiddleRight;    // Must return something
                return false;
            }
            else
            {
                finger = FingerExtensions.Import(fingerToken)
                    ?? throw new FormatException($"Undefined finger ('{fingerToken}') not allowed for finger position definition!");

                return true;
            }
        }
    }
}