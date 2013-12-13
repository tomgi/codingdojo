using System;

namespace Ajejczes.CodingDojo.StringCalculator
{
    public class StringCalculator
    {
        public int Add(string input)
        {
            var delimiters = new[] {',', '\n'};
            if (input.StartsWith("//"))
            {
                delimiters = new[] {input[2]};
                input = input.Substring(3);
            }

            var nums = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int sum = 0;
            foreach (var num in nums)
                sum += int.Parse(num);
            return sum;
        }
    }
}