using System;

namespace Ajejczes.CodingDojo
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
            {
                int element = int.Parse(num);
                if (element < 0)
                    throw new ApplicationException("negatives not allowed");
                sum += element;
            }
            ;
            return sum;
        }
    }
}