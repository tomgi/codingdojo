using System;
using System.Diagnostics.Contracts;
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
        }
    }

    internal static class IntToRomanConverter
    {
        // I II III IV V VI VII VIII IX X
        // XI XII XIII XIV XVI XVII XVIII XIX XX

        public static string ToRoman(this int i)
        {
            var result = string.Empty;
            int tens;
            var magnitude = 10;
            return RomanPolanski(i, magnitude, "I", "V", "X");
        }

        static string RomanPolanski(int i, int magnitude, string elem_I, string elem_V, string elem_X)
        {
            string result = string.Empty;

            int mag_05 = magnitude / 2;
            int mag_04 = (int) (magnitude * .4);
            int mag_09 = (int) (magnitude * .9);

            int mag_count = i / magnitude;

            var biggerMagnitude = mag_count * magnitude;

            if (mag_count >= 4)
                result += RomanPolanski(biggerMagnitude, magnitude * 10, "X", "L", "C");
            else
                result += string.Join(string.Empty, Enumerable.Repeat(elem_X, mag_count));

            i -= biggerMagnitude;

            if (i % mag_05 == mag_04)
                result += elem_I;
            if (i >= mag_09)
                result += elem_X;
            else if (i >= mag_04)
                result += elem_V;
            return result + string.Join(string.Empty, Enumerable.Repeat(elem_I, i % mag_05 != mag_04 ? i % mag_05 : 0));
        }
    }
}