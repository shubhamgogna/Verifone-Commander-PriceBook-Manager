// -----------------------------------------------------------------------
// <copyright file="Department.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    using System.Collections.Generic;

    public class Department : ICloneable<Department>
    {
        public int SystemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool AllowFoodStamps { get; set; }

        public int ProductCodeId { get; set; }

        public ISet<int> TaxRateIds { get; set; } = new HashSet<int>();

        public ISet<int> AgeValidationIds { get; set; } = new HashSet<int>();

        public Department Clone()
        {
            return new Department
            {
                SystemId = this.SystemId,
                Name = this.Name,
                AllowFoodStamps = this.AllowFoodStamps,
                ProductCodeId = this.ProductCodeId,
                TaxRateIds = new HashSet<int>(this.TaxRateIds),
                AgeValidationIds = new HashSet<int>(this.AgeValidationIds),
            };
        }
    }
}
