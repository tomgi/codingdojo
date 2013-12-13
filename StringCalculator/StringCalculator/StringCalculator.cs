using System;
using System.Linq;

namespace StringCalculatorKata
{
    public class StringCalculator
    {
        public int Add(string input)
        {
            var nums = input.Split(",\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int sum = 0;
            foreach (var num in nums)
                sum += int.Parse(num);
            return sum;
        }
    }
}