using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Infra.Utils
{
    public static class TokenGenerator
    {
        public static List<int> GenerateToken(List<int> numbers, int rotations)
        {
            for (int i = 0; i < rotations; i++)
            {
                var lastNumber = numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
                numbers.Insert(0, lastNumber);
            }

            return numbers;
        }

        public static long GetTokenFromCardNumber(string cardNumber, int rotations)
        {
            var lastCardDigits = cardNumber.Substring(cardNumber.Length - 4).Select(digit => int.Parse(digit.ToString())).ToList();
            var tokenList = GenerateToken(lastCardDigits, rotations);
            return int.Parse(string.Join(",", tokenList).Replace(",", ""));
        }
    }
}
