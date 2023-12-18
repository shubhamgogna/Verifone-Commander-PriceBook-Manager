// -----------------------------------------------------------------------
// <copyright file="AgeValidation.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    public class AgeValidation : ICloneable<AgeValidation>
    {
        public int SystemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public AgeValidation Clone()
        {
            return new AgeValidation
            {
                SystemId = this.SystemId,
                Name = this.Name,
            };
        }
    }
}
