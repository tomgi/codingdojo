using System;
using System.Linq;
using System.Linq.Expressions;
using NSpec;

namespace IHS.CodingDojo.IntToRoman
{

    public class IntToRomanSpecs : nspec
    {
        void when_integer_is_provided()
        {
            specify = () => 1.ToRoman().should_be("I");
            specify = () => 2.ToRoman().should_be("II");
            specify = () => 3.ToRoman().should_be("III");
            specify = () => 4.ToRoman().should_be("IV");
            specify = () => 5.ToRoman().should_be("V");
            specify = () => 6.ToRoman().should_be("VI");
            specify = () => 7.ToRoman().should_be("VII");
            specify = () => 8.ToRoman().should_be("VIII");
            specify = () => 9.ToRoman().should_be("IX");
            specify = () => 10.ToRoman().should_be("X");
            specify = () => 14.ToRoman().should_be("XIV");
            specify = () => 15.ToRoman().should_be("XV");
            specify = () => 19.ToRoman().should_be("XIX");
            specify = () => 20.ToRoman().should_be("XX");
            specify = () => 34.ToRoman().should_be("XXXIV");
            specify = () => 40.ToRoman().should_be("XL");
            specify = () => 41.ToRoman().should_be("XLI");
            specify = () => 41.ToRoman().should_be("XLI");
            specify = () => 99.ToRoman().should_be("XCIX");
            specify = () => 999.ToRoman().should_be("CMXCIX");
            specify = () => 1999.ToRoman().should_be("MCMXCIX");
            specify = () => 2999.ToRoman().should_be("MMCMXCIX");

        }
    }

    internal static class IntToRomanConverter
    {
        static readonly Tuple<string, string, string>[] RomanTuples =
        {
            new Tuple<string, string, string>("I","X","V"), 
            new Tuple<string, string, string>("X","C","L"), 
            new Tuple<string, string, string>("C","M","D")
        };

        public static string ToRoman(this int i)
        {
            var result = string.Empty;

            foreach (var romanTuple in RomanTuples)
            {
                result = (i % 10).RomanPolanski(romanTuple.Item1, romanTuple.Item2, romanTuple.Item3) + result;
                i = i / 10;
            }
            result = string.Join(string.Empty, Enumerable.Repeat("M", i)) + result;
            return result;
        }

        static string RomanPolanski(this int i, string minor, string major, string middle)
        {
            var result = string.Empty;
            if (i == 4)
                result += minor + middle;
            if (i == 9)
                result += minor + major;
            else if (i >= 5)
                result += middle;
            if (i % 5 <= 3)
                result += string.Join(string.Empty, Enumerable.Repeat(minor, i % 5));
            return result;
        }
    }
}