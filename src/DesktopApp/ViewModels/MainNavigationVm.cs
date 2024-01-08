// -----------------------------------------------------------------------
// <copyright file="MainNavigationVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;

    public partial class MainNavigationVm : ViewModelBase
    {
        private readonly ObservableCollection<IPageVM> headerPages = new();
        private readonly ObservableCollection<IPageVM> footerPages = new();

        [ObservableProperty]
        private IPageVM currentPage;

        public MainNavigationVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            IModifiableSapphireCredentialsProvider credentialsProvider,
            ISapphireClient sapphireClient)
            : base(uiThreadDispatcher, messenger, logger)
        {
            var settings = new Settings();

            this.AccountPage = new AccountPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                settings,
                credentialsProvider,
                sapphireClient);

            this.SettingsPage = new SettingsPageVm(
                settings,
                uiThreadDispatcher,
                messenger,
                logger);

            this.headerPages.Add(this.AccountPage);
            this.footerPages.Add(this.SettingsPage);

            this.CurrentPage = this.AccountPage;
        }

        public ObservableCollection<IPageVM> HeaderPages => this.headerPages;

        public ObservableCollection<IPageVM> FooterPages => this.footerPages;

        public AccountPageVm AccountPage { get; private set; }

        public SettingsPageVm SettingsPage { get; private set; }
    }
}
