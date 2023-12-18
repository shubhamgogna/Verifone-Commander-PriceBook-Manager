// -----------------------------------------------------------------------
// <copyright file="SearchPageVm.cs" company="Shubham Gogna">
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
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.Core.Models;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models;

    public partial class SearchPageVm : PageVm
    {
        private readonly ISapphireClient sapphireClient;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private bool canSearch = true;

        [ObservableProperty]
        private ObservableCollection<SearchPageItemVm> searchResults = new ObservableCollection<SearchPageItemVm>();

        public SearchPageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            ISapphireClient sapphireClient)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.sapphireClient = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            this.SearchCommand = new AsyncRelayCommand(this.SearchAsync);
        }

        public override string Name => "Search";

        // Search
        public override int SymbolCode => 0xE721;

        public ICommand SearchCommand { get; }

        private static bool DoesPluHaveSearchText(
            Plu plu,
            Department department,
            string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            if (plu.Ean13.ToString("D13").Contains(searchText))
            {
                return true;
            }

            if (plu.Modifier.ToString("D3").Contains(searchText))
            {
                return true;
            }

            if (plu.Description != null &&
                plu.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (department != null &&
                department.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private async Task SearchAsync(CancellationToken cancellationToken)
        {
            if (!this.CanSearch)
            {
                return;
            }

            await this.DispatchOnUiThreadAsync(() => this.CanSearch = false).ConfigureAwait(false);

            var matchingItems = new List<SearchPageItemVm>();

            // Query for all PLUs
            var plus = await this.sapphireClient.GetPriceLookUpsAsync(cancellationToken).ConfigureAwait(false);

            // Match
            foreach (var plu in plus)
            {
                var department = await this.sapphireClient.GetDepartmentByIdAsync(
                    plu.DepartmentId,
                    cancellationToken).ConfigureAwait(false);

                if (DoesPluHaveSearchText(plu, department, this.SearchText))
                {
                    matchingItems.Add(new SearchPageItemVm
                    {
                        Ean13 = plu.Ean13,
                        Modifier = plu.Modifier,
                        Price = plu.Price,
                        Description = plu.Description,
                        Department = department?.Name,
                        EditCommand = new RelayCommand(() =>
                        {
                            this.Messenger.Send(new LoadProductForEditMessage(plu.Ean13, plu.Modifier));
                        }),
                    });
                }
            }

            // Sort
            var orderedItems = matchingItems
                .OrderBy(x => x.Ean13)
                .ThenBy(x => x.Modifier)
                .ToList();

            // Update
            await this.DispatchOnUiThreadAsync(() =>
            {
                this.SearchResults = new ObservableCollection<SearchPageItemVm>(orderedItems);
                this.CanSearch = true;
            }).ConfigureAwait(false);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type

    public partial class SearchPageItemVm : ObservableObject
    {
        [ObservableProperty]
        private long ean13;

        [ObservableProperty]
        private int modifier;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private double price;

        [ObservableProperty]
        private string department;

        [ObservableProperty]
        private ICommand editCommand;
    }
}
