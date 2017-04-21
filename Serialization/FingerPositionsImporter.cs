﻿using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using System;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerPositionsImporter : FingerPositionsBasePorter, IStringImport<FingerPositions>
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

            var lines = exportedString.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var contents = line.Split(new[] { Separator + KeySeparator }, StringSplitOptions.RemoveEmptyEntries);
                if (contents.Length < 2)
                {
                    continue;
                }

                if (!GetFinger(throwExceptions, contents[0], out Finger? finger))
                {
                    return false;
                }

                if (!GetTokensFromLine(throwExceptions, contents[1], out string[] tokens))
                {
                    return false;
                }

                if (finger != null)
                {
                    foreach (var token in tokens)
                    {
                        if (!GetKeyPosition(throwExceptions, token, out KeyPosition keyPosition))
                        {
                            return false;
                        }

                        // ReSharper disable once PossibleInvalidOperationException
                        positionsDict.Add(keyPosition, finger.Value);
                    }
                }
            }

            outputInstance = new FingerPositions(positionsDict);
            return true;
        }

        private static bool GetTokensFromLine(bool throwExceptions, string line, out string[] tokens)
        {
            tokens = line.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length >= 1)
            {
                return true;
            }
            else
            {
                if (throwExceptions)
                {
                    throw new FormatException($"Couldn't parse finger positions line '{line}'!");
                }
                return false;
            }
        }

        private static bool GetKeyPosition(bool throwExceptions, string token, out KeyPosition keyPosition)
        {
            keyPosition = default(KeyPosition);

            var indexes = token.Split(new[] { RowKeySeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (indexes.Length == 2)
            {
                var rowIndexString = indexes[0];
                if (!int.TryParse(rowIndexString, out int iRow))
                {
                    return throwExceptions
                        ? false
                        : throw new FormatException(
                            $"Couldn't parse '{rowIndexString}' to a row index e.g. '2'");
                }

                var keyIndexString = indexes[1];
                if (!int.TryParse(keyIndexString, out int iKey))
                {
                    return throwExceptions
                        ? false
                        : throw new FormatException(
                            $"Couldn't parse '{keyIndexString}' to a key index e.g. '10'");
                }

                keyPosition = new KeyPosition(iRow, iKey);
                return true;
            }
            else
            {
                return throwExceptions
                    ? false
                    : throw new FormatException(
                        $"Couldn't parse '{token}' to a row/key index pair e.g. '2{RowKeySeparator}10'");
            }
        }

        private static bool GetFinger(bool throwExceptions, string fingerToken, out Finger? finger)
        {
            if (!throwExceptions)
            {
                return FingerExtensions.TryImport(fingerToken, out finger) && (finger != null);
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