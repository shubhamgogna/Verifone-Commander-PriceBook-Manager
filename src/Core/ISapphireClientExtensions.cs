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
        public static async Task<Plu> GetPriceLookUpAsync(
            this ISapphireClient sapphireClient,
            long ean13,
            int modifier,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.Ean13 == ean13 && x.Modifier == modifier);
        }

        public static async Task<Department> GetDepartmentByIdAsync(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetDepartmentsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<Department> GetDepartmentByNameAsync(
            this ISapphireClient sapphireClient,
            string name,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetDepartmentsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public static async Task<TaxRate> GetTaxRateByIdAsync(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetTaxRatesAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<TaxRate> GetTaxRateByNameAsync(
            this ISapphireClient sapphireClient,
            string name,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetTaxRatesAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public static async Task<AgeValidation> GetAgeValidationByIdAsync(
            this ISapphireClient sapphireClient,
            int id,
            CancellationToken cancellationToken)
        {
            _ = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            var list = await sapphireClient.GetAgeValidationsAsync(cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(x => x.SystemId == id);
        }

        public static async Task<AgeValidation> GetAgeValidationByNameAsync(
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
