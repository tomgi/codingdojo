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

        }
    }

    internal static class IntToRomanConverter
    {
        // I II III IV V VI VII VIII IX X
        // XI XII XIII XIV XVI XVII XVIII XIX XX

        public static string ToRoman(this int i)
        {
            var result = string.Empty;
            var tens = i/10;
            
            i -= tens*10;
            result += string.Join(string.Empty,Enumerable.Repeat("X", tens));
            if (i%5 == 4)
                result += "I";
            if (i >= 9)
                result += "X";
            else if (i >= 4)
                result += "V";
            return result + string.Join(string.Empty, Enumerable.Repeat("I", i%5 != 4 ? i % 5 : 0));
        }
    }
}