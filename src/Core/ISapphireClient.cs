// -----------------------------------------------------------------------
// <copyright file="ISapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public interface ISapphireClient : IDisposable
    {
        Task<List<Plu>> GetPriceLookUpsAsync();

        Task UpdatePriceLookUpAsync(Plu plu);

        Task DeletePriceLookUpAsync(long ean13, int modifier);

        Task<List<Department>> GetDepartmentsAsync();

        Task<List<TaxRate>> GetTaxRatesAsync();

        Task<List<AgeValidation>> GetAgeValidationsAsync();
    }
}
