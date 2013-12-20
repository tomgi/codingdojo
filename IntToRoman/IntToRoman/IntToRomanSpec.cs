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
            int tens;
            var magnitude = 10;
            return RomanPolanski(i, magnitude, "I", "X", "V");
        }

        static string RomanPolanski(int i, int magnitude, string elem_I, string elem_X, string elem_V)
        {
            string result= string.Empty;
            int magcount;
            
            int V_quant = magnitude/2;
            var V_lesser = magnitude * .4;
            var X_Lesser = magnitude * .9;

            magcount = i/magnitude;

            if (magcount >= 4)
            {
                result += RomanPolanski(i, magnitude*10, "X", "C", "L");
                i -= magcount * magnitude; 
                magcount = 0;
            }

            i -= magcount*magnitude;
            result += string.Join(string.Empty, Enumerable.Repeat(elem_X, magcount));

            if (i%V_quant == V_lesser)
                result += elem_I;
            if (i >= X_Lesser)
                result += elem_X;
            else if (i >= V_lesser)
                result += elem_V;
            return result + string.Join(string.Empty, Enumerable.Repeat(elem_I, i%V_quant != V_lesser ? i%V_quant : 0));
        }
    }
}