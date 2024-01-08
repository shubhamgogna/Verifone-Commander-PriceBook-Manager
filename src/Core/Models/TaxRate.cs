// -----------------------------------------------------------------------
// <copyright file="TaxRate.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    public class TaxRate : ICloneable<TaxRate>
    {
        public int SystemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Rate { get; set; }

        public TaxRate Clone()
        {
            return new TaxRate
            {
                SystemId = this.SystemId,
                Name = this.Name,
                Rate = this.Rate,
            };
        }
    }
}
