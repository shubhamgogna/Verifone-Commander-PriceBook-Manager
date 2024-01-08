// -----------------------------------------------------------------------
// <copyright file="ISapphireClientExtensions.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public static class ISapphireClientExtensions
    {
        public static async Task<Department> GetDepartmentById(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetDepartmentsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<Department> GetDepartmentByName(
            this ISapphireClient sapphireClient,
            string name,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetDepartmentsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public static async Task<TaxRate> GetTaxRateById(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetTaxRatesAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<TaxRate> GetTaxRateByName(
            this ISapphireClient sapphireClient,
            string name,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetTaxRatesAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public static async Task<AgeValidation> GetAgeValidationById(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetAgeValidationsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<AgeValidation> GetAgeValidationByName(
            this ISapphireClient sapphireClient,
            string name,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetAgeValidationsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }
    }
}
