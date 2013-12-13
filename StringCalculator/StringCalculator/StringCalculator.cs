using System;
using System.Collections.Generic;

namespace Ajejczes.CodingDojo
{
    public class StringCalculator
    {
        readonly char[] _defaultDelimiters = new[] { ',', '\n' };

        public int Add(string input)
        {
            var numbers = ParseStringInput(input);

            EnsureNumbersDoNotContainNegatives(numbers);

            return Add(numbers);
        }

        static int Add(List<int> numbers)
        {
            int sum = 0;
            foreach (var element in numbers)
                sum += element;
            return sum;
        }

        static void EnsureNumbersDoNotContainNegatives(List<int> numbers)
        {
            var negativeNums = new List<int>();
            foreach (var number in numbers)
            {
                if (number < 0)
                    negativeNums.Add(number);
            }

            if (negativeNums.Count > 0)
            {
                throw new ApplicationException(
                    String.Format(
                        "Negatives not allowed: {0}",
                        String.Join(",", negativeNums)));
            }
        }

        List<int> ParseStringInput(string input)
        {
            var delimiters = _defaultDelimiters;
            if (input.StartsWith("//"))
            {
                delimiters = new []{input[2]};
                input = input.Substring(3);
            }

            var nums = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            var numbers = new List<int>();
            foreach (var num in nums)
                numbers.Add(int.Parse(num));
            return numbers;
        }
    }
}