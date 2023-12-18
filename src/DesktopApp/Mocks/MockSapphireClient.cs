// -----------------------------------------------------------------------
// <copyright file="MockSapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public class MockSapphireClient : ISapphireClient
    {
        private List<Plu> plus;
        private List<Department> departments = new List<Department>()
        {
            new Department
            {
                SystemId = 1,
                AllowFoodStamps = true,
                Name = "GROCERY",
            },
            new Department
            {
                SystemId = 2,
                Name = "NON GROCERY",
                TaxRateIds = new HashSet<int>() { 1, 2 },
            },
            new Department
            {
                SystemId = 3,
                Name = "ALCOHOL",
                TaxRateIds = new HashSet<int>() { 1, 2 },
                AgeValidationIds = new HashSet<int>() { 1 },
            },
            new Department
            {
                SystemId = 4,
                Name = "TOBACCO",
                TaxRateIds = new HashSet<int>() { 1, 2 },
                AgeValidationIds = new HashSet<int>() { 2 },
            },
        };

        private List<TaxRate> taxRates = new List<TaxRate>()
        {
            new TaxRate
            {
                SystemId = 1,
                Name = "TAX LOCAL",
            },
            new TaxRate
            {
                SystemId = 2,
                Name = "TAX FEDERAL",
            },
        };

        private List<AgeValidation> ageValidations = new List<AgeValidation>()
        {
            new AgeValidation
            {
                SystemId = 1,
                Name = "ALCOHOL ID CHECK",
            },
            new AgeValidation
            {
                SystemId = 2,
                Name = "TOBACCO ID CHECK",
            },
        };

        public MockSapphireClient()
        {
            this.plus = this.GenerateRandomPlus(10_0);
        }

        public async Task<List<Plu>> GetPriceLookUpsAsync(
            CancellationToken cancellationToken)
        {
            await DelayAsync().ConfigureAwait(false);
            return this.plus.Select(x => x.Clone()).ToList();
        }

        public async Task<List<Department>> GetDepartmentsAsync(
            CancellationToken cancellationToken)
        {
            await DelayAsync().ConfigureAwait(false);
            return this.departments.Select(x => x.Clone()).ToList();
        }

        public async Task<List<TaxRate>> GetTaxRatesAsync(
            CancellationToken cancellationToken)
        {
            await DelayAsync().ConfigureAwait(false);
            return this.taxRates.Select(x => x.Clone()).ToList();
        }

        public async Task<List<AgeValidation>> GetAgeValidationsAsync(
            CancellationToken cancellationToken)
        {
            await DelayAsync().ConfigureAwait(false);
            return this.ageValidations.Select(x => x.Clone()).ToList();
        }

        public async Task UpdatePriceLookUpAsync(
            Plu plu,
            CancellationToken cancellationToken)
        {
            _ = plu ?? throw new ArgumentNullException(nameof(plu));

            await DelayAsync().ConfigureAwait(false);

            var index = this.plus.FindIndex(x => x.Ean13 == plu.Ean13 && x.Modifier == plu.Modifier);

            if (index >= 0)
            {
                this.plus[index] = plu.Clone();
            }
            else
            {
                this.plus.Add(plu.Clone());
            }
        }

        public async Task DeletePriceLookUpAsync(
            long ean13,
            int modifier,
            CancellationToken cancellationToken)
        {
            await DelayAsync().ConfigureAwait(false);

            var index = this.plus.FindIndex(x => x.Ean13 == ean13 && x.Modifier == modifier);
            if (index >= 0)
            {
                this.plus.RemoveAt(index);
            }
        }

        private static Task DelayAsync()
        {
            // Simulate some async network operation by using an async delay
            return Task.Delay(500);
        }

        private List<Plu> GenerateRandomPlus(int numToGenerate)
        {
            var list = new List<Plu>();
            var random = new Random();

            for (int i = 0; i < numToGenerate; i++)
            {
                var departmentId = random.Next(0, this.departments.Count) + 1; // IDs start at 1
                var price = random.NextDouble() * 100;

                var taxRateIdsToGenerate = random.Next(0, this.taxRates.Count);
                var taxRateIds = new HashSet<int>();
                for (int j = 0; j <= taxRateIdsToGenerate; j++)
                {
                    var id = random.Next(0, this.taxRates.Count) + 1; // IDs start at 1
                    taxRateIds.Add(id);
                }

                var ageValidationsToGenerate = random.Next(0, this.ageValidations.Count);
                var ageValidationIds = new HashSet<int>();
                for (int j = 0; j <= ageValidationsToGenerate; j++)
                {
                    var id = random.Next(0, this.ageValidations.Count) + 1; // IDs start at 1
                    ageValidationIds.Add(id);
                }

                list.Add(new Plu
                {
                    Ean13 = Ean13Helper.ConvertToEan13WithCheckDigit(i),
                    Modifier = 0,
                    Description = $"Item #{i}",
                    DepartmentId = departmentId,
                    ProductCodeId = 400,
                    Price = price,
                    TaxRateIds = taxRateIds,
                    AgeValidationIds = ageValidationIds,
                });
            }

            return list;
        }
    }
}
