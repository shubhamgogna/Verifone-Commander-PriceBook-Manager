// -----------------------------------------------------------------------
// <copyright file="Plu.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    using System.Collections.Generic;

    public class Plu : ICloneable<Plu>
    {
        public long Ean13 { get; set; }

        public int Modifier { get; set; }

        public string Description { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public ISet<int> FeeIds { get; set; } = new HashSet<int>() { 0 };

        public int ProductCodeId { get; set; }

        public double Price { get; set; }

        public ISet<int> FlagIds { get; set; } = new HashSet<int>() { 1, 5 };

        public ISet<int> TaxRateIds { get; set; } = new HashSet<int>();

        public ISet<int> AgeValidationIds { get; set; } = new HashSet<int>();

        public double SellUnit { get; set; } = 1.00D;

        public double TaxableRebateAmount { get; set; }

        public double MaxQuantityPerTransaction { get; set; }

        public static ISet<int> GenerateDefaultFeeIds()
        {
            return new HashSet<int>() { 0 };
        }

        public static ISet<int> GenerateDefaultFlagIds()
        {
            return new HashSet<int>() { 1, 5 };
        }

        public Plu Clone()
        {
            return new Plu
            {
                Ean13 = this.Ean13,
                Modifier = this.Modifier,
                Description = this.Description,
                DepartmentId = this.DepartmentId,
                FeeIds = new HashSet<int>(this.FeeIds),
                ProductCodeId = this.ProductCodeId,
                Price = this.Price,
                FlagIds = new HashSet<int>(this.FlagIds),
                TaxRateIds = new HashSet<int>(this.TaxRateIds),
                AgeValidationIds = new HashSet<int>(this.AgeValidationIds),
                SellUnit = this.SellUnit,
                TaxableRebateAmount = this.TaxableRebateAmount,
                MaxQuantityPerTransaction = this.MaxQuantityPerTransaction,
            };
        }
    }
}
