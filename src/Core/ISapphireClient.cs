// -----------------------------------------------------------------------
// <copyright file="ISapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public interface ISapphireClient
    {
        Task<List<Plu>> GetPriceLookUpsAsync(
            CancellationToken cancellationToken);

        Task UpdatePriceLookUpAsync(
            Plu plu,
            CancellationToken cancellationToken);

        Task DeletePriceLookUpAsync(
            long ean13,
            int modifier,
            CancellationToken cancellationToken);

        Task<List<Department>> GetDepartmentsAsync(
            CancellationToken cancellationToken);

        Task<List<TaxRate>> GetTaxRatesAsync(
            CancellationToken cancellationToken);

        Task<List<AgeValidation>> GetAgeValidationsAsync(
            CancellationToken cancellationToken);
    }
}
