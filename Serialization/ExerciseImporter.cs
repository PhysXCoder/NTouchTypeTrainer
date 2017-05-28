using NTouchTypeTrainer.Common.LINQ;
using NTouchTypeTrainer.Common.RegexExtensions;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Domain.Exercises;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NTouchTypeTrainer.Serialization
{
    public class ExerciseImporter : BaseImporter, IStringImport<IExercise>
    {
        protected const string ExerciseType = "ExerciseType";
        protected const string ExerciseText = "ExerciseText";
        protected const string ExerciseDescription = "ExerciseDescription";
        protected const string DefaultCultureInfo = "default";
        protected const string CultureDelimeter = "--";
        protected const string ManualDefinitionDelimeter = "--";
        protected const string BlockContentSeparator = ":";
        protected const string BlockCountSeparator = "x";
        protected const string GroupSeparator = " ";

        public static bool TryImport(string exportedString, out IExercise outputExercise)
            => Import(exportedString, false, out outputExercise);

        public static IExercise Import(string exportedString)
        {
            Import(exportedString, true, out IExercise exercise);
            return exercise;
        }

        IExercise IStringImport<IExercise>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<IExercise>.TryImport(string exportedString, out IExercise outputInstance)
            => TryImport(exportedString, out outputInstance);

        private static bool Import(string exportedString, bool throwExceptions, out IExercise outputExercise)
        {
            outputExercise = null;
            if (exportedString == null)
            {
                throw new ArgumentNullException(nameof(exportedString));
            }

            var exerciseCoarseStructureRegex = new Regex(
                $"(--{ExerciseType}--([\\r\\n]*)(?<{ExerciseType}>[\\s\\S]*?)([\\r\\n]*))"
                + $"(--{ExerciseText}--([\\r\\n]*)(?<{ExerciseText}>[\\s\\S]*?)([\\r\\n]*))"
                + $"(--{ExerciseDescription}--([\\r\\n]*)(?<{ExerciseDescription}>[\\s\\S]*))");
            var match = exerciseCoarseStructureRegex.Match(exportedString);

            string exerciseTypeString, exerciseTextString, exerciseDescriptionString;
            if (throwExceptions)
            {
                exerciseTypeString = match.GetGroupContent(ExerciseType);
                exerciseTextString = match.GetGroupContent(ExerciseText);
                exerciseDescriptionString = match.GetGroupContent(ExerciseDescription);
            }
            else
            {
                var success = match.TryGetGroupContent(ExerciseType, out exerciseTypeString);
                success &= match.TryGetGroupContent(ExerciseText, out exerciseTextString);
                success &= match.TryGetGroupContent(ExerciseDescription, out exerciseDescriptionString);

                if (!success)
                {
                    return false;
                }
            }

            if (!GetExerciseType(exerciseTypeString, throwExceptions, out ExerciseType exerciseType))
            {
                return false;
            }

            if (!GetDescriptions(
                    exerciseDescriptionString,
                    throwExceptions,
                    out IDictionary<CultureInfo, string> descriptionsDict))
            {
                return false;
            }

            switch (exerciseType)
            {
                case Domain.Enums.ExerciseType.Fix:
                    return ImportFixedExercise(exerciseTextString, descriptionsDict, out outputExercise);

                case Domain.Enums.ExerciseType.RandomizedBlocks:
                    return ImportRandomizedBlocksExercise(exerciseTextString, descriptionsDict, throwExceptions, out outputExercise);

                default:
                    throw new NotImplementedException($"No import routine for exercise type {exerciseType} available!");
            }
        }

        private static bool GetExerciseType(string exerciseTypeString, bool throwExceptions, out ExerciseType exerciseType)
        {
            if (throwExceptions)
            {
                exerciseType = ExerciseTypeExtensions.Parse(exerciseTypeString);
            }
            else
            {
                if (!ExerciseTypeExtensions.TryParse(exerciseTypeString, out exerciseType))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool GetDescriptions(
            string exerciseDescriptionString,
            bool throwExceptions,
            out IDictionary<CultureInfo, string> descriptions)
        {
            const string cultureGroup = "cultureInfo";
            const string descriptionGroup = "description";

            var descriptionRegex = new Regex($"{CultureDelimeter}(?<{cultureGroup}>([\\s\\S])*?){CultureDelimeter}(?<{descriptionGroup}>((?!{CultureDelimeter})[\\s\\S])*)(\\r?\\n)?");
            var allMatches = descriptionRegex.Matches(exerciseDescriptionString);

            descriptions = new Dictionary<CultureInfo, string>();
            foreach (Match match in allMatches)
            {
                string cultureText, descriptionText;
                if (throwExceptions)
                {
                    cultureText = match.GetGroupContent(cultureGroup);
                    descriptionText = match.GetGroupContent(descriptionGroup);
                }
                else
                {
                    var parseSuccess = true;
                    parseSuccess &= match.TryGetGroupContent(descriptionGroup, out descriptionText);
                    parseSuccess &= match.TryGetGroupContent(cultureGroup, out cultureText);

                    if (!parseSuccess)
                    {
                        return false;
                    }
                }

                var isDefaultCulture = DefaultCultureInfo.Equals(cultureText, StringComparison.InvariantCulture);
                var culture = !isDefaultCulture ? new CultureInfo(cultureText) : CultureInfo.InvariantCulture;

                descriptions.Add(culture, descriptionText);
            }

            return true;
        }

        private static bool ImportRandomizedBlocksExercise(
            string exerciseText,
            IDictionary<CultureInfo, string> descriptionsDict,
            bool throwExceptions,
            out IExercise outputExercise)
        {
            exerciseText = RemoveAllButOneWhitespaceAtEnd(exerciseText);

            var parseSuccess = ParseBlocks(exerciseText, throwExceptions, out ICollection<ExerciseBlock> exerciseBlocks);

            outputExercise = parseSuccess ? new RandomizedBlocksExercise(descriptionsDict, exerciseBlocks) : null;

            return parseSuccess;
        }

        private static bool ImportFixedExercise(
            string exerciseText,
            IDictionary<CultureInfo, string> descriptionsDict,
            out IExercise outputExercise)
        {
            exerciseText = RemoveAllButOneWhitespaceAtEnd(exerciseText);

            var mappedKeys = ParseMappingTargets(exerciseText);

            outputExercise = new FixedExercise(descriptionsDict, mappedKeys);

            return true;
        }

        private static IList<IMappingTarget> ParseMappingTargets(string exerciseText)
        {
            var mappingTargets = new List<IMappingTarget>();

            var iNext = 0;
            while (iNext < exerciseText.Length)
            {
                var isSpecial = TryParseSpecial(exerciseText, ref iNext, out IMappingTarget mappingTarget);

                IMappingTarget target;
                if (isSpecial)
                {
                    target = mappingTarget;
                }
                else
                {
                    var character = exerciseText[iNext];
                    target = new MappedCharacter(character);
                    iNext++;
                }

                mappingTargets.Add(target);
            }

            return mappingTargets;
        }

        private static bool TryParseSpecial(string exerciseText, ref int iStart, out IMappingTarget mappingTarget)
        {
            mappingTarget = null;
            if (TryGetManualDefinitionText(exerciseText, iStart, out string specialText, out int specialLength))
            {
                var parseSpecialSuccessfull = TryParseManualDefinition(specialText, out mappingTarget);

                if (parseSpecialSuccessfull)
                {
                    iStart += specialLength;
                    return true;
                }
            }
            else if (IsNewline(exerciseText, iStart, out int lengthNewline))
            {
                mappingTarget = new MappedHardwareKey(HardwareKey.Enter);
                iStart += lengthNewline;
                return true;
            }
            else if (IsTab(exerciseText, iStart, out int lengthTab))
            {
                mappingTarget = new MappedHardwareKey(HardwareKey.Tab);
                iStart += lengthTab;
                return true;
            }

            return false;
        }

        private static bool TryGetManualDefinitionText(
            string exerciseText,
            int iStart,
            out string definitionText,
            out int definitionLength)
        {
            definitionText = "";
            definitionLength = 0;

            var longEnough = (exerciseText.Length - iStart > 2 * ManualDefinitionDelimeter.Length);
            if (longEnough)
            {
                var definitionStartFound = ManualDefinitionDelimeter.Equals(exerciseText.Substring(iStart, ManualDefinitionDelimeter.Length));
                var iDefinitionStart = iStart + ManualDefinitionDelimeter.Length;

                var iDefinitionEnd = exerciseText.IndexOf(
                    ManualDefinitionDelimeter,
                    iStart + ManualDefinitionDelimeter.Length,
                    StringComparison.InvariantCultureIgnoreCase);
                var definitionEndFound = iDefinitionEnd > iStart;

                if (definitionStartFound && definitionEndFound)
                {
                    definitionText = exerciseText.Substring(iDefinitionStart, iDefinitionEnd - iDefinitionStart);
                    definitionLength = iDefinitionEnd - iStart + ManualDefinitionDelimeter.Length;
                    return true;
                }
            }

            return false;
        }

        // Examples for text: "Ctrl+Space", "Space", "Ctrl+AltGr+A"
        private static bool TryParseManualDefinition(string text, out IMappingTarget mappingTarget)
        {
            mappingTarget = null;

            var keyFound = TryParseModifiersAndHardwareKey(text, out Modifier modifiers, out HardwareKey key);

            if (keyFound)
            {
                var isPrintableKey = (key.ToString().Length == 1);
                mappingTarget = isPrintableKey && modifiers == Modifier.None
                    ? (IMappingTarget)new MappedCharacter(key.ToString()[0])
                    : new MappedHardwareKey(key, modifiers);

                return true;
            }

            return false;
        }

        private static bool TryParseModifiersAndHardwareKey(string text, out Modifier modifiers, out HardwareKey key)
        {
            key = HardwareKey.None;
            modifiers = Modifier.None;
            var keyFound = false;

            var tokens = text.Split(new[] { ModifierExtensions.ModifierCombiner }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                var isModifier = Enum.TryParse(token, true, out Modifier currentModifier);
                if (isModifier)
                {
                    modifiers |= currentModifier;
                }
                else
                {
                    if (!TryUniquelyParseHardwareKey(token, ref keyFound, out key))
                    {
                        keyFound = false;
                        break;
                    }
                }
            }

            return keyFound;
        }

        private static bool TryUniquelyParseHardwareKey(string token, ref bool keyFound, out HardwareKey key)
        {
            var isHardwareKey = Enum.TryParse(token, true, out key);
            if (isHardwareKey)
            {
                var doubleKeyDefinition = keyFound;
                if (doubleKeyDefinition)
                {
                    return false;
                }

                keyFound = true;
                return true;
            }

            keyFound = false;
            return false;
        }

        private static bool IsNewline(string exerciseText, int iStart, out int length)
            => MatchesAnyString(exerciseText, iStart, new[] { "\n", "\r\n" }, out length);

        private static bool IsTab(string exerciseText, int iStart, out int length)
            => MatchesAnyString(exerciseText, iStart, new[] { "\t" }, out length);

        private static bool MatchesAnyString(string exerciseText, int iStart, IEnumerable<string> stringsToMatch, out int length)
        {
            var substringToCheck = exerciseText.Substring(iStart);

            foreach (var matchCandidate in stringsToMatch.OrderByDescending(s => s.Length))
            {
                if (substringToCheck.StartsWith(matchCandidate))
                {
                    length = matchCandidate.Length;
                    return true;
                }
            }

            length = 0;
            return false;
        }

        private static string RemoveAllButOneWhitespaceAtEnd(string text)
        {
            var regex = new Regex("(?<content>[\\S\\s]*?)(?<end>\\r?\\n)?$");
            var match = regex.Match(text);

            var trimmedContent = match.GetGroupContent("content").TrimEnd();

            match.TryGetGroupContent("end", out string end);

            return trimmedContent + end;
        }

        private static bool ParseBlocks(string exerciseText, bool throwExceptions, out ICollection<ExerciseBlock> exerciseBlocks)
        {
            exerciseBlocks = new List<ExerciseBlock>();

            var blockLines = exerciseText.Split(new[] { RowSeparator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in blockLines)
            {
                var parseSuccess = ParseExerciseBlock(line, throwExceptions, out ExerciseBlock block);
                if (parseSuccess)
                {
                    exerciseBlocks.Add(block);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ParseExerciseBlock(string line, bool throwExceptions, out ExerciseBlock exerciseBlock)
        {
            exerciseBlock = null;

            var iBlockContentSeparator = line.IndexOf(BlockContentSeparator, StringComparison.InvariantCulture);
            var iBlockCountSeparator = line.IndexOf(BlockCountSeparator, StringComparison.InvariantCulture);

            if (iBlockCountSeparator < 0 && iBlockContentSeparator < 0)
            {
                return throwExceptions
                    ? throw new FormatException($"Wrong format for ExerciseBlock. Expected: <NumberOfLines>x<NumberOfGroupsPerLine>:<Group1> <Group2> ... Example: '1x6:asdf 1234'")
                    : false;
            }

            var rowCountString = line.Substring(0, iBlockCountSeparator);
            var groupCountString = line.Substring(
                iBlockCountSeparator + 1,
                iBlockCountSeparator - iBlockCountSeparator + 1);

            var rowCount = int.Parse(rowCountString);
            var groupCount = int.Parse(groupCountString);
            var groups = ParseBlockGroups(line.Substring(iBlockContentSeparator + 1));

            exerciseBlock = new ExerciseBlock(rowCount, groupCount, groups);
            return true;
        }

        private static IEnumerable<IEnumerable<IMappingTarget>> ParseBlockGroups(string blockContent)
        {
            IList<IEnumerable<IMappingTarget>> groups = new List<IEnumerable<IMappingTarget>>();

            var groupStrings = blockContent.Split(new[] { GroupSeparator }, StringSplitOptions.RemoveEmptyEntries);

            groupStrings.ForEach(groupString =>
                groups.Add(ParseMappingTargets(groupString)));


            return groups;
        }
    }
}