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
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models;

    public partial class MainNavigationVm : ViewModelBase, IRecipient<LoginStateChangedMessage>, IRecipient<LoadProductForEditMessage>
    {
        private readonly ObservableCollection<PageVm> headerPages = new();
        private readonly ObservableCollection<PageVm> footerPages = new();

        [ObservableProperty]
        private PageVm currentPage;

        [ObservableProperty]
        private InfoBarVm infoBar = new InfoBarVm();

        public MainNavigationVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            Settings settings,
            IModifiableSapphireCredentialsProvider credentialsProvider,
            ICachingSapphireClient sapphireClient,
            IFileSystem fileSystem)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.AccountPage = new AccountPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                settings,
                credentialsProvider,
                sapphireClient);

            this.BulkOperationsPage = new BulkOperationsPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                sapphireClient,
                fileSystem);

            this.EditPage = new EditPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                sapphireClient);

            this.SearchPlusPage = new SearchPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                sapphireClient);

            this.SettingsPage = new SettingsPageVm(
                uiThreadDispatcher,
                messenger,
                logger,
                settings);

            this.headerPages.Add(this.AccountPage);
            this.footerPages.Add(this.SettingsPage);

            // Initial page
            this.CurrentPage = this.AccountPage;

            // Messenger
            this.Messenger.Register<LoginStateChangedMessage>(this);
            this.Messenger.Register<LoadProductForEditMessage>(this);
        }

        public ObservableCollection<PageVm> HeaderPages => this.headerPages;

        public ObservableCollection<PageVm> FooterPages => this.footerPages;

        public AccountPageVm AccountPage { get; private set; }

        public BulkOperationsPageVm BulkOperationsPage { get; private set; }

        public EditPageVm EditPage { get; private set; }

        public SearchPageVm SearchPlusPage { get; private set; }

        public SettingsPageVm SettingsPage { get; private set; }

        void IRecipient<LoginStateChangedMessage>.Receive(LoginStateChangedMessage message)
        {
            if (message.State == LoginState.LoggedIn)
            {
                this.HeaderPages.Add(this.SearchPlusPage);
                this.HeaderPages.Add(this.EditPage);
                this.HeaderPages.Add(this.BulkOperationsPage);

                this.CurrentPage = this.SearchPlusPage;
            }
            else if (message.State == LoginState.LoggedOut)
            {
                this.CurrentPage = this.AccountPage;

                this.HeaderPages.Remove(this.SearchPlusPage);
                this.HeaderPages.Remove(this.EditPage);
                this.HeaderPages.Remove(this.BulkOperationsPage);
            }
        }

        void IRecipient<LoadProductForEditMessage>.Receive(LoadProductForEditMessage message)
        {
            this.CurrentPage = this.EditPage;
        }
    }
}
