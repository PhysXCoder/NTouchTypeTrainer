using System.Collections.Generic;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class FingerExtensions
    {
        public static Dictionary<Finger?, string> FingerToString;
        public static Dictionary<string, Finger?> StringToFinger;

        private const string UndefinedFinger = "UD";

        static FingerExtensions()
        {
            FingerToString = new Dictionary<Finger?, string>()
            {
                {Finger.SmallLeft, "SL"},
                {Finger.RingLeft, "RL"},
                {Finger.MiddleLeft, "ML"},
                {Finger.IndexLeft, "IL"},
                {Finger.Thumb, "T"},
                {Finger.IndexRight, "IR"},
                {Finger.MiddleRight, "MR"},
                {Finger.RingRight, "RR"},
                {Finger.SmallRight, "SR"},
            };

            StringToFinger = new Dictionary<string, Finger?>();
            foreach (var keyValuePair in FingerToString)
            {
                StringToFinger.Add(keyValuePair.Value, keyValuePair.Key);
            }
            StringToFinger.Add(UndefinedFinger, null);
        }

        public static Finger? Import(string exportedString)
        {
            Finger? finger;
            if (TryImport(exportedString, out finger))
            {
                return finger;
            }

            throw new KeyNotFoundException($"No finger is associated with string '{exportedString}'!");
        }

        public static bool TryImport(string exportedString, out Finger? finger)
        {
            exportedString = exportedString?.Trim();
            if ((exportedString != null) && StringToFinger.ContainsKey(exportedString))
            {
                finger = StringToFinger[exportedString];
                return true;
            }

            finger = null;
            return false;
        }
    }
}