using System;
using System.Collections.Generic;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using static NTouchTypeTrainer.Domain.Enums.HardwareKeyExtensions;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsImporter : FingerPositionsBasePorter, IFingerPositionsImporter
    {
        public bool TryImport(string exportedString, out FingerPositions outputInstance)
        {
            outputInstance = null;
            var positionsDict = new Dictionary<HardwareKey, Finger>();

            using (var rowEnumerator = AllRows.GetEnumerator())
            {
                var lines = exportedString.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (!rowEnumerator.MoveNext())
                    {
                        break;
                    }
                    var row = rowEnumerator.Current;
                    if (row == null)
                    {
                        break;
                    }

                    using (var keyEnumerator = row.GetEnumerator())
                    {
                        var fingers = line.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var fingerString in fingers)
                        {
                            if (!keyEnumerator.MoveNext())
                            {
                                break;
                            }

                            Finger? finger;
                            var parseSuccess = FingerExtensions.TryImport(fingerString, out finger);                            
                            if (!parseSuccess)
                            {
                                return false;
                            }

                            if (finger != null)
                            {
                                var key = keyEnumerator.Current;
                                positionsDict.Add(key, finger.Value);
                            }
                        }
                    }
                }
            }

            outputInstance = new FingerPositions(positionsDict);
            return true;
        }

        public FingerPositions Import(string exportedString)
        {
            FingerPositions fingerPos;
            var parseSuccess = TryImport(exportedString, out fingerPos);

            if (parseSuccess)
            {
                return fingerPos;
            }
            else
            {
                throw new FormatException($"Couldn't parse finger positions '{exportedString}'!");
            }
        }
    }
}