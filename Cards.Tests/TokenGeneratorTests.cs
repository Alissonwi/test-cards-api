using Cards.Infra.Utils;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cards.Tests
{
    public class TokenGeneratorTests
    {
        [Theory]
        [InlineData(15489314567851234, 3412, 2)]
        [InlineData(15489314567851444, 4441, 3)]
        [InlineData(15489314567851135, 1135, 4)]
        public void ReturnTokenFromACardNumberAndCVV(
            long cardNumber,
            long expectedToken,
            int CVV)
        {
            var generatedToken = TokenGenerator.GetTokenFromCardNumber(cardNumber.ToString(), CVV);

            Assert.Equal(expectedToken, generatedToken);
        }

        [Theory]
        [InlineData(new int[] {4, 5, 1, 2, 3})]
        public void ReturnListFromAIntegerListAfterRoutatingTheNumbersFromListInArrange(
            int[] expectedTokenList)
        {
            List<int> numbers = new();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);
            numbers.Add(4);
            numbers.Add(5);

            var generatedTokenList = TokenGenerator.GenerateToken(numbers, 2);

            Assert.Equal(expectedTokenList.ToList(), generatedTokenList);
        }

        [Theory]
        [InlineData(new int[] { 3, 4, 5 }, new int[] { 4, 5, 3 }, 2)]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 2, 3, 1 }, 2)]
        public void ReturnListFromAIntegerListAfterRoutatingTheNumbersFromList(
            int[] parameterList,
            int[] expectedList,
            int rotations)
        {
            var generatedList = TokenGenerator.GenerateToken(parameterList.ToList(), rotations);

            Assert.Equal(expectedList.ToList(), generatedList);
        }
    }
}
