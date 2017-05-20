using System;

namespace NTouchTypeTrainer.Domain.Enums
{
    public class ExerciseTypeExtensions
    {
        public static bool TryParse(string exportedString, out ExerciseType exerciseType, bool ignoreCase = true)
        {
            return Enum.TryParse(exportedString, ignoreCase, out exerciseType);
        }

        public static ExerciseType Parse(string exportedString, bool ignoreCase = true)
        {
            if (!TryParse(exportedString, out ExerciseType exerciseType, ignoreCase))
            {
                throw new FormatException($"Couldn't parse exercise type '{exportedString}'!");
            }

            return exerciseType;
        }
    }
}