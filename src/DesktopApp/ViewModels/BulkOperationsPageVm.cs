// -----------------------------------------------------------------------
// <copyright file="BulkOperationsPageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using System.Collections.Generic;
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

    public partial class BulkOperationsPageVm : PageVm
    {
        private readonly ISapphireClient sapphireClient;
        private readonly IFileSystem fileSystem;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SyncWithDepartmentCommand))]
        [NotifyCanExecuteChangedFor(nameof(CapitalizeDescriptionsCommand))]
        [NotifyCanExecuteChangedFor(nameof(BackupCommand))]
        private bool canExecuteCommands = true;

        public BulkOperationsPageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            ISapphireClient sapphireClient,
            IFileSystem fileSystem)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.sapphireClient = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            this.SyncWithDepartmentCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.SyncWithDepartmentAsync, ct),
                () => this.CanExecuteCommands);

            this.CapitalizeDescriptionsCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.CapitalizeDescriptionsAsync, ct),
                () => this.CanExecuteCommands);

            this.BackupCommand = new AsyncRelayCommand(
                ct => this.RunIfCanExecuteCommandsAsync(this.BackupAsync, ct),
                () => this.CanExecuteCommands);
        }

        public override string Name => "Bulk Operations";

        // Set
        public override int SymbolCode => 0xF5ED;

        public IRelayCommand SyncWithDepartmentCommand { get; }

        public IRelayCommand CapitalizeDescriptionsCommand { get; }

        public IRelayCommand BackupCommand { get; }

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

        private async Task SyncWithDepartmentAsync(CancellationToken cancellationToken)
        {
            const string FixName = "Sync With Department";

            var successCount = 0;
            var failCount = 0;

            var plus = await this.sapphireClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false);
            foreach (var plu in plus)
            {
                var department = await this.sapphireClient.GetDepartmentByIdAsync(
                    plu.DepartmentId,
                    cancellationToken).ConfigureAwait(false);

                if (department == null)
                {
                    successCount++;
                    continue;
                }

                plu.TaxRateIds = new HashSet<int>(department.TaxRateIds);
                plu.AgeValidationIds = new HashSet<int>(department.AgeValidationIds);

                if (department.AllowFoodStamps)
                {
                    plu.FlagIds.Add(PluFlags.FoodStamps);
                }
                else
                {
                    plu.FlagIds.Remove(PluFlags.FoodStamps);
                }

                try
                {
                    // Update
                    await this.sapphireClient.UpdatePriceLookUpAsync(plu, cancellationToken).ConfigureAwait(false);
                    successCount++;
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "Exception");
                    failCount++;
                }

                await this.DispatchOnUiThreadAsync(() =>
                {
                    this.SetInfoBar(
                        InfoBarSeverity.Informational,
                        $"[{FixName}] {successCount}/{plus.Count} succeded. {failCount}/{plus.Count} failed.");
                }).ConfigureAwait(false);
            }

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"[{FixName}] {successCount}/{plus.Count} succeded. {failCount}/{plus.Count} failed.");
            }).ConfigureAwait(false);
        }

        private async Task CapitalizeDescriptionsAsync(CancellationToken cancellationToken)
        {
            const string FixName = "Capitalize Description";

            var successCount = 0;
            var failCount = 0;

            var plus = await this.sapphireClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false);
            foreach (var plu in plus)
            {
                var newDescription = plu.Description.Trim().ToUpperInvariant();

                if (!string.Equals(plu.Description, newDescription, StringComparison.Ordinal))
                {
                    plu.Description = newDescription;

                    try
                    {
                        // Update
                        await this.sapphireClient.UpdatePriceLookUpAsync(plu, cancellationToken).ConfigureAwait(false);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        this.Logger.LogError(ex, "Exception");
                        failCount++;
                    }
                }

                await this.DispatchOnUiThreadAsync(() =>
                {
                    this.SetInfoBar(
                        InfoBarSeverity.Informational,
                        $"[{FixName}] {successCount}/{plus.Count} succeded. {failCount}/{plus.Count} failed.");
                }).ConfigureAwait(false);
            }

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"[{FixName}] {successCount}/{plus.Count} succeded. {failCount}/{plus.Count} failed.");
            }).ConfigureAwait(false);
        }

        private async Task BackupAsync(CancellationToken cancellationToken)
        {
            const string FixName = "Backup";

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.SetInfoBar(
                    InfoBarSeverity.Informational,
                    $"[{FixName}] Running ...");
            }).ConfigureAwait(false);

            var plus = await this.sapphireClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false);
            var convertedPlus = plus
                .Select(x => ModelConverter.ConvertPluToXml(x))
                .ToArray();

            var element = new XElement(
                SapphireXNames.Plus,
                new XAttribute(SapphireXNames.DomainNamespace, SapphireXNames.DomainNamespaceName),
                convertedPlus);

            var fileName = $"backup-{DateTime.UtcNow:s}.xml".Replace(":", "-");
            this.fileSystem.FileWriteAllText(fileName, element.ToString());

            // Queue on UI thread
            await this.DispatchOnUiThreadAsync(() =>
            {
                this.SetInfoBar(
                    InfoBarSeverity.Success,
                    $"[{FixName}] Finished");
            }).ConfigureAwait(false);
        }
    }
}
