using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoCToolbox
{
    public static class StringExtensions
    {
        private static readonly Regex NumberRegex = new(pattern: @"(-?\d+)");
        private static readonly Regex WhitespaceRegex = new(pattern: @"\s+");

        public static IList<int> ParseInts(this string str)
        {
            var matches = NumberRegex.Matches(str);
            var numbers = new List<int>(matches.Select(m => int.Parse(m.Value)));

            return numbers;
        }

        public static int ParseInt(this string str)
        {
            return ParseInts(str)[0];
        }

        public static long ParseLong(this string str)
        {
            return ParseLongs(str)[0];
        }

        public static IList<long> ParseLongs(this string str)
        {
            var matches = NumberRegex.Matches(str);
            var numbers = new List<long>(matches.Select(m => long.Parse(m.Value)));

            return numbers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsDigit(this char c)
        {
            return c - '0';
        }

        public static string RemoveWhitespace(this string str)
        {
            return WhitespaceRegex.Replace(input: str, replacement: string.Empty);
        }

        public static T ParseNumber<T>(this string s) where T : INumber<T>
        {
            return ParseNumbers<T>(s)[0];
        }

        public static T[] ParseNumbers<T>(this string s) where T : INumber<T>
        {
            return NumberRegex.Matches(s)
                .Select(m => T.Parse(s: m.Value.AsSpan(), provider: null))
                .ToArray();
        }

        /// <summary>
        /// Extracts all "Words" (including xnoppyt) from a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IEnumerable<string> ExtractWords(this string str)
        {
            return Regex.Matches(str, "[a-zA-z]+").Select(a => a.Value);
        }

    }
}
