// -----------------------------------------------------------------------
// <copyright file="CachingSapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public class CachingSapphireClient : ICachingSapphireClient
    {
        private readonly ISapphireClient innerClient;

        private List<Plu> plus = new List<Plu>();
        private List<Department> departments = new List<Department>();
        private List<TaxRate> taxRates = new List<TaxRate>();
        private List<AgeValidation> ageValidations = new List<AgeValidation>();

        public CachingSapphireClient(
            ISapphireClient innerClient)
        {
            this.innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
        }

        public async Task RefreshCacheAsync(CancellationToken cancellationToken)
        {
            this.plus = await this.innerClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false) ?? new List<Plu>();
            this.departments = await this.innerClient.GetDepartmentsAsync(cancellationToken).ConfigureAwait(false) ?? new List<Department>();
            this.taxRates = await this.innerClient.GetTaxRatesAsync(cancellationToken).ConfigureAwait(false) ?? new List<TaxRate>();
            this.ageValidations = await this.innerClient.GetAgeValidationsAsync(cancellationToken).ConfigureAwait(false) ?? new List<AgeValidation>();
        }

        public Task<List<Plu>> GetPriceLookUpsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.plus.Select(x => x.Clone()).ToList());
        }

        public async Task UpdatePriceLookUpAsync(
            Plu plu,
            CancellationToken cancellationToken)
        {
            _ = plu ?? throw new ArgumentNullException(nameof(plu));

            await this.innerClient.UpdatePriceLookUpAsync(plu, cancellationToken).ConfigureAwait(false);

            var index = this.plus.FindIndex(x => x.Ean13 == plu.Ean13 && x.Modifier == plu.Modifier);
            if (index >= 0)
            {
                this.plus[index] = plu;
            }
            else
            {
                this.plus.Add(plu);
            }
        }

        public async Task DeletePriceLookUpAsync(long ean13, int modifier, CancellationToken cancellationToken)
        {
            await this.innerClient.DeletePriceLookUpAsync(ean13, modifier, cancellationToken).ConfigureAwait(false);

            var index = this.plus.FindIndex(x => x.Ean13 == ean13 && x.Modifier == modifier);
            if (index >= 0)
            {
                this.plus.RemoveAt(index);
            }
        }

        public Task<List<Department>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.departments.Select(x => x.Clone()).ToList());
        }

        public Task<List<TaxRate>> GetTaxRatesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.taxRates.Select(x => x.Clone()).ToList());
        }

        public Task<List<AgeValidation>> GetAgeValidationsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.ageValidations.Select(x => x.Clone()).ToList());
        }
    }
}
