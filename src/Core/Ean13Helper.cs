// -----------------------------------------------------------------------
// <copyright file="Ean13Helper.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;

    /// <summary>
    /// EAN = European Article Number (also known as International Article Number)
    /// https://en.wikipedia.org/wiki/International_Article_Number
    /// </summary>
    public static class Ean13Helper
    {
        public const int Ean13Length = 13;

        private const long MaxEan13 = 9_999_999_999_999; // [12 digits] + [Check digit]
        private const long MaxEan13WithoutCheckDigit = MaxEan13 / 10;

        public static bool CanBeEan13(long upc)
        {
            return upc <= MaxEan13;
        }

        public static long ConvertToEan13WithCheckDigit(long ean13WithoutCheckDigit)
        {
            if (ean13WithoutCheckDigit > MaxEan13WithoutCheckDigit)
            {
                throw new ArgumentOutOfRangeException(nameof(ean13WithoutCheckDigit), $"Value must be less than {MaxEan13WithoutCheckDigit} when there is no check digit");
            }

            long temp = ean13WithoutCheckDigit;
            int sum = 0;

            // Need to compute a sum using 12 digits
            for (int i = 1; i <= 12; i++)
            {
                // Get last digit
                var digit = (int)(temp % 10);

                if (i % 2 == 0)
                {
                    sum += digit;
                }
                else
                {
                    sum += 3 * digit;
                }

                // "Shift" right
                temp /= 10;
            }

            var m = sum % 10;
            var checkDigit = m == 0 ? 0 : 10 - m;

            // "Append" check digit
            return (ean13WithoutCheckDigit * 10) + checkDigit;
        }
    }
}
