using System;
using System.Collections.Generic;

namespace Ajejczes.CodingDojo
{
    public class StringCalculator
    {
        const string DelimiterTag = "//";
        readonly string[] _defaultDelimiters = new[] {",", "\n"};

        public int Add(string input)
        {
            var numbers = ParseStringInput(input);

            EnsureNumbersDoNotContainNegatives(numbers);

            numbers = RemoveBigNumbers(numbers);

            return Add(numbers);
        }

        List<int> RemoveBigNumbers(List<int> numbers)
        {
            var ret = new List<int>();
            foreach (var number in numbers)
            {
                if (number < 1000)
                    ret.Add(number);
            }
            return ret;
        }

        static int Add(List<int> numbers)
        {
            var sum = 0;
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
            string[] delimiters;
            string numbersLine;
            ExtractsDelimitersAndNumbers(input, out delimiters, out numbersLine);

            var nums = numbersLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            var numbers = new List<int>();
            foreach (var num in nums)
                numbers.Add(int.Parse(num));
            return numbers;
        }

        void ExtractsDelimitersAndNumbers(string input, out string[] delimiters, out string numbersLine)
        {
            if (input.StartsWith(DelimiterTag))
            {
                var delimiterAndNumbers = input.Split(new char[] {'\n'}, 2);
                var delimiterLine = delimiterAndNumbers[0].Substring(DelimiterTag.Length);
                numbersLine = delimiterAndNumbers[1];

                if (delimiterLine[0] == '[')
                    delimiters = new[] {delimiterLine.Substring(1, delimiterLine.Length - 1 - 1)};
                else
                    delimiters = new[] {delimiterLine};
            }
            else
            {
                numbersLine = input;
                delimiters = _defaultDelimiters;
            }
        }
    }
}