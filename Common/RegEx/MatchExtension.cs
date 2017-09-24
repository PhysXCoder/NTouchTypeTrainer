using System;
using System.Text.RegularExpressions;

namespace NTouchTypeTrainer.Common.RegEx
{
    public static class MatchExtension
    {
        public static string GetGroupContent(this Match match, string groupName)
        {
            GetGroupContent(match, groupName, true, out string content);
            return content;
        }

        public static bool TryGetGroupContent(this Match match, string groupName, out string content)
            => GetGroupContent(match, groupName, false, out content);

        private static bool GetGroupContent(
            this Match match,
            string groupName,
            bool throwExceptions,
            out string content)
        {
            content = null;
            if (match == null)
            {
                return throwExceptions ? throw new ArgumentNullException(nameof(match)) : false;
            }
            if (groupName == null)
            {
                return throwExceptions ? throw new ArgumentNullException(nameof(groupName)) : false;
            }

            if (!match.Success)
            {
                return throwExceptions
                    ? throw new FormatException($"Couldn't parse regular expression. Group '{groupName}' not found!")
                    : false;
            }

            content = match.Groups[groupName].Value;
            return true;
        }
    }
}