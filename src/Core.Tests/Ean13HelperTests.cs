// -----------------------------------------------------------------------
// <copyright file="Ean13HelperTests.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Tests
{
    using Xunit;

    public class Ean13HelperTests
    {
        [Theory]
        [InlineData("03600029145", "036000291452")]
        [InlineData("3400000035", "34000000357")]
        [InlineData("03400000035", "34000000357")]
        public void ConvertToEan13WithCheckDigit(
            string upcaStringWithoutCheckDigit,
            string upcaStringWithCheckDigit)
        {
            var upcaWithoutCheckDigit = long.Parse(upcaStringWithoutCheckDigit);
            var expectedUpcaWithCheckDigit = long.Parse(upcaStringWithCheckDigit);

            var actualUpcaWithCheckDigit = Ean13Helper.ConvertToEan13WithCheckDigit(upcaWithoutCheckDigit);
            Assert.Equal(expectedUpcaWithCheckDigit, actualUpcaWithCheckDigit);
        }
    }
}
