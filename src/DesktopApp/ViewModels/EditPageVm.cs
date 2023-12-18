// -----------------------------------------------------------------------
// <copyright file="EditPageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.Core.Models;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models;

    public partial class EditPageVm : PageVm, IRecipient<LoginStateChangedMessage>, IRecipient<LoadProductForEditMessage>
    {
        private readonly ISapphireClient sapphireClient;

        private Dictionary<string, DepartmentInfo> departmentInfoDict = new Dictionary<string, DepartmentInfo>();

        [ObservableProperty]
        private ValidatedTextVm ean;

        [ObservableProperty]
        private ValidatedTextVm modifier;

        [ObservableProperty]
        private ValidatedTextVm description;

        [ObservableProperty]
        private ValidatedTextVm price;

        [ObservableProperty]
        private ObservableCollection<string> departmentNames = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedDepartmentName;

        [ObservableProperty]
        private ObservableCollection<string> taxRateNames = new ObservableCollection<string>();

        [ObservableProperty]
        private ObservableCollection<string> ageValidationNames = new ObservableCollection<string>();

        [ObservableProperty]
        private bool allowFoodStamps;

        [ObservableProperty]
        private double sellUnit = 1;

        [ObservableProperty]
        private double taxableRebateAmount;

        [ObservableProperty]
        private double maxQuantityPerTransaction;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private bool canExecuteCommands = true;

        public EditPageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            ISapphireClient sapphireClient)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.sapphireClient = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            this.Ean = new ValidatedTextVm(
                validationFunc: v =>
                {
                    if (string.IsNullOrEmpty(v))
                    {
                        return "EAN cannot be empty";
                    }

                    v = v.Trim();

                    if (!long.TryParse(v, out long parsedEan13))
                    {
                        return "EAN must be some number";
                    }

                    if (v.Length != Ean13Helper.Ean13Length)
                    {
                        return $"EAN must be {Ean13Helper.Ean13Length} digits";
                    }

                    if (!Ean13Helper.CanBeEan13(parsedEan13))
                    {
                        return "EAN must be in the valid EAN range";
                    }

                    var ean13WithoutCheckDigit = parsedEan13 / 10;
                    var recomputedEan13 = Ean13Helper.ConvertToEan13WithCheckDigit(ean13WithoutCheckDigit);

                    if (parsedEan13 != recomputedEan13)
                    {
                        return "EAN check-digit does not match";
                    }

                    return string.Empty;
                },
                initialValue: string.Empty,
                afterTextChangeFunc: this.AddEanCheckDigitToText);

            this.Modifier = new ValidatedTextVm(
                validationFunc: v =>
                {
                    if (string.IsNullOrEmpty(v))
                    {
                        return "Modifier cannot be empty";
                    }

                    if (!int.TryParse(v, out int _))
                    {
                        return "Modifier must be some number";
                    }

                    return string.Empty;
                },
                initialValue: "0");

            this.Description = new ValidatedTextVm(
                v =>
                {
                    if (string.IsNullOrEmpty(v))
                    {
                        return "Description cannot be empty";
                    }

                    return string.Empty;
                });

            this.Price = new ValidatedTextVm(
                validationFunc: v =>
                {
                    if (string.IsNullOrEmpty(v))
                    {
                        return "Price cannot be empty";
                    }

                    if (!double.TryParse(v, out double _))
                    {
                        return "Price must be some decimal value";
                    }

                    return string.Empty;
                },
                initialValue: "0.00");

            this.LoadCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.LoadProductAsync, ct),
                () => this.CanExecuteCommands);

            this.SaveCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.SaveProductAsync, ct),
                () => this.CanExecuteCommands);

            this.DeleteCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.DeleteProductAsync, ct),
                () => this.CanExecuteCommands);

            // Messenger
            this.Messenger.Register<LoginStateChangedMessage>(this);
            this.Messenger.Register<LoadProductForEditMessage>(this);
        }

        public override string Name => "Edit";

        // Edit
        public override int SymbolCode => 0xE104;

        public IRelayCommand LoadCommand { get; }

        public IRelayCommand SaveCommand { get; }

        public IRelayCommand DeleteCommand { get; }

        void IRecipient<LoginStateChangedMessage>.Receive(LoginStateChangedMessage message)
        {
            if (message.State != LoginState.LoggedIn)
            {
                return;
            }

            Task.Run(async () =>
            {
                var departments = await this.sapphireClient.GetDepartmentsAsync(default).ConfigureAwait(false);
                var taxRates = await this.sapphireClient.GetTaxRatesAsync(default).ConfigureAwait(false);
                var ageValidations = await this.sapphireClient.GetAgeValidationsAsync(default).ConfigureAwait(false);

                var departmentInfoDict = new Dictionary<string, DepartmentInfo>();
                foreach (var department in departments)
                {
                    var taxRateNames = department
                        .TaxRateIds
                        .Select(id => taxRates.FirstOrDefault(tr => tr.SystemId == id))
                        .Where(x => x != null)
                        .Select(x => x.Name)
                        .OrderBy(x => x)
                        .ToArray();

                    var ageValidationNames = department
                        .AgeValidationIds
                        .Select(id => ageValidations.FirstOrDefault(tr => tr.SystemId == id))
                        .Where(x => x != null)
                        .Select(x => x.Name)
                        .OrderBy(x => x)
                        .ToArray();

                    departmentInfoDict[department.Name] = new DepartmentInfo
                    {
                        AllowFoodStamps = department.AllowFoodStamps,
                        TaxRateNames = taxRateNames,
                        AgeValidationNames = ageValidationNames,
                    };
                }

                await this.DispatchOnUiThreadAsync(() =>
                {
                    // Save dict
                    this.departmentInfoDict = departmentInfoDict;

                    // Set department names
                    this.DepartmentNames.Clear();
                    foreach (var x in departmentInfoDict)
                    {
                        this.DepartmentNames.Add(x.Key);
                    }

                    // Select first if available
                    if (this.DepartmentNames.Count > 0)
                    {
                        this.SelectedDepartmentName = this.DepartmentNames.FirstOrDefault();
                    }
                }).ConfigureAwait(false);
            });
        }

        void IRecipient<LoadProductForEditMessage>.Receive(LoadProductForEditMessage message)
        {
            Task.Run(async () =>
            {
                await this.LoadProductAsync(message.Ean13, message.Modifier, default).ConfigureAwait(false);
            });
        }

        partial void OnSelectedDepartmentNameChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (this.departmentInfoDict.TryGetValue(value, out var info))
            {
                this.AllowFoodStamps = info.AllowFoodStamps;

                this.TaxRateNames.Clear();
                foreach (var n in info.TaxRateNames)
                {
                    this.TaxRateNames.Add(n);
                }

                this.AgeValidationNames.Clear();
                foreach (var n in info.AgeValidationNames)
                {
                    this.AgeValidationNames.Add(n);
                }
            }
        }

        private async Task RunIfCanExecuteCommandsAsync(Func<CancellationToken, Task> func, CancellationToken cancellationToken)
        {
            if (!this.CanExecuteCommands)
            {
                return;
            }

            this.CanExecuteCommands = false;

            try
            {
                await func(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await this.DispatchOnUiThreadAsync(() => this.CanExecuteCommands = true).ConfigureAwait(false);
            }
        }

        private Task LoadProductAsync(
            CancellationToken cancellationToken)
        {
            if (this.Ean.HasError_Revalidate() ||
                this.Modifier.HasError_Revalidate())
            {
                return Task.CompletedTask;
            }

            var ean13 = long.Parse(this.Ean.Text);
            var modifier = int.Parse(this.Modifier.Text);

            return this.LoadProductAsync(ean13, modifier, cancellationToken);
        }

        private async Task LoadProductAsync(
            long ean13,
            int modifier,
            CancellationToken cancellationToken)
        {
            //// For thread safety: No access to bindable object properties beyond this point.

            // Query
            var plu = await this.sapphireClient.GetPriceLookUpAsync(
                ean13,
                modifier,
                cancellationToken).ConfigureAwait(false);

            if (plu == null)
            {
                await this.DispatchOnUiThreadAsync(() =>
                {
                    this.ClearVm();

                    this.SetInfoBar(
                        InfoBarSeverity.Error,
                        $"Unable to find EAN-13 '{ean13:D13}' with modifier '{modifier:D3}'");
                }).ConfigureAwait(false);

                return;
            }

            var department = await this.sapphireClient.GetDepartmentByIdAsync(
                plu.DepartmentId,
                cancellationToken).ConfigureAwait(false);

            var taxRateNames = new HashSet<string>();
            foreach (var id in plu.TaxRateIds)
            {
                var taxRate = await this.sapphireClient.GetTaxRateByIdAsync(
                    id,
                    cancellationToken).ConfigureAwait(false);

                taxRateNames.Add(taxRate.Name);
            }

            var ageValidationNames = new HashSet<string>();
            foreach (var id in plu.AgeValidationIds)
            {
                var ageValidation = await this.sapphireClient.GetAgeValidationByIdAsync(
                    id,
                    cancellationToken).ConfigureAwait(false);

                ageValidationNames.Add(ageValidation.Name);
            }

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.UpdateVm(
                    plu,
                    department.Name ?? string.Empty,
                    taxRateNames,
                    ageValidationNames);

                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"Loaded EAN-13 '{ean13:D13}' with modifier '{modifier:D3}'");
            }).ConfigureAwait(false);
        }

        private async Task SaveProductAsync(CancellationToken cancellationToken)
        {
            if (this.Ean.HasError_Revalidate() ||
                this.Modifier.HasError_Revalidate() ||
                this.Price.HasError_Revalidate() ||
                this.Description.HasError_Revalidate())
            {
                return;
            }

            // Clone properties before running async operations
            var price = double.Parse(this.Price.Text);
            var description = this.Description.Text;
            var selectedDepartmentName = this.SelectedDepartmentName;

            var ean13 = long.Parse(this.Ean.Text);
            var modifier = int.Parse(this.Modifier.Text);

            //// For thread safety: No access to bindable object properties beyond this point.

            var department = await this.sapphireClient.GetDepartmentByNameAsync(
                selectedDepartmentName,
                cancellationToken).ConfigureAwait(false);

            var flagIds = Plu.GenerateDefaultFlagIds();

            if (department.AllowFoodStamps)
            {
                flagIds.Add(PluFlags.FoodStamps);
            }
            else
            {
                flagIds.Remove(PluFlags.FoodStamps);
            }

            var plu = new Plu()
            {
                Ean13 = ean13,
                Modifier = modifier,
                Description = description,
                DepartmentId = department.SystemId,
                ProductCodeId = department.ProductCodeId,
                Price = price,
                FlagIds = flagIds,
                TaxRateIds = department.TaxRateIds,
                AgeValidationIds = department.AgeValidationIds,
            };

            // Update
            await this.sapphireClient.UpdatePriceLookUpAsync(
                plu,
                cancellationToken).ConfigureAwait(false);

            // Since the PLU was overwritten with the new properties of
            // the department, the IDs (and names) of the tax rates and
            // age validations may have changed.
            var taxRateNames = new HashSet<string>();
            foreach (var id in plu.TaxRateIds)
            {
                var taxRate = await this.sapphireClient.GetTaxRateByIdAsync(
                    id,
                    cancellationToken).ConfigureAwait(false);

                taxRateNames.Add(taxRate.Name);
            }

            var ageValidationNames = new HashSet<string>();
            foreach (var id in plu.AgeValidationIds)
            {
                var ageValidation = await this.sapphireClient.GetAgeValidationByIdAsync(
                    id,
                    cancellationToken).ConfigureAwait(false);

                ageValidationNames.Add(ageValidation.Name);
            }

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.UpdateVm(
                    plu,
                    department.Name ?? string.Empty,
                    taxRateNames,
                    ageValidationNames);

                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"Saved EAN-13 '{ean13:D13}' with modifier '{modifier:D3}'");
            }).ConfigureAwait(false);
        }

        private async Task DeleteProductAsync(CancellationToken cancellationToken)
        {
            if (this.Ean.HasError_Revalidate() ||
                this.Modifier.HasError_Revalidate())
            {
                return;
            }

            var ean13 = long.Parse(this.Ean.Text);
            var modifier = int.Parse(this.Modifier.Text);

            //// For thread safety: No access to bindable object properties beyond this point.

            // Delete
            await this.sapphireClient.DeletePriceLookUpAsync(
                ean13,
                modifier,
                cancellationToken).ConfigureAwait(false);

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.ClearVm();

                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"Deleted EAN-13 '{ean13:D13}' with modifier '{modifier:D3}'");
            }).ConfigureAwait(false);
        }

        private void ClearVm()
        {
            this.Ean.Text = string.Empty;
            this.Modifier.Text = "0";
            this.Description.Text = string.Empty;
            this.SelectedDepartmentName = null;
            this.Price.Text = "0.00";
            this.AllowFoodStamps = false;
            this.TaxRateNames.Clear();
            this.AgeValidationNames.Clear();
            this.SellUnit = 1;
            this.TaxableRebateAmount = 0;
            this.MaxQuantityPerTransaction = 0;
        }

        private void UpdateVm(
            Plu plu,
            string departmentName,
            HashSet<string> taxRateNames,
            HashSet<string> ageValidationNames)
        {
            this.Ean.Text = plu.Ean13.ToString("D13");
            this.Modifier.Text = plu.Modifier.ToString();
            this.Description.Text = plu.Description;
            this.SelectedDepartmentName = departmentName;
            this.Price.Text = plu.Price.ToString("F2");
            this.AllowFoodStamps = plu.FlagIds.Contains(PluFlags.FoodStamps);

            this.TaxRateNames.Clear();
            foreach (var name in taxRateNames)
            {
                this.TaxRateNames.Add(name);
            }

            this.AgeValidationNames.Clear();
            foreach (var name in ageValidationNames)
            {
                this.AgeValidationNames.Add(name);
            }

            this.SellUnit = 1;
            this.TaxableRebateAmount = 0;
            this.MaxQuantityPerTransaction = 0;
        }

        private void AddEanCheckDigitToText()
        {
            if (this.Ean.Text == null)
            {
                return;
            }

            var text = this.Ean.Text.Trim();

            if (text.Length != 12)
            {
                return;
            }

            if (!long.TryParse(text, out var parsedEan13WithoutCheckDigit))
            {
                return;
            }

            var recomputedEan13 = Ean13Helper.ConvertToEan13WithCheckDigit(parsedEan13WithoutCheckDigit);
            this.Ean.Text = recomputedEan13.ToString("D13");
        }

        private class DepartmentInfo
        {
            public bool AllowFoodStamps { get; set; }

            public IReadOnlyList<string> TaxRateNames { get; set; }

            public IReadOnlyList<string> AgeValidationNames { get; set; }
        }
    }
}
