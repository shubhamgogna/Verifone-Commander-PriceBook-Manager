// -----------------------------------------------------------------------
// <copyright file="MainNavigationVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;

    public partial class MainNavigationVm : ObservableObject
    {
        private readonly ObservableCollection<IPageVM> headerPages = new();
        private readonly ObservableCollection<IPageVM> footerPages = new();

        [ObservableProperty]
        private IPageVM currentPage;

        public MainNavigationVm()
        {
            var settings = new Settings();
            this.AccountPage = new LoginPageVm(settings);
            this.SettingsPage = new SettingsPageVm(settings);

            this.headerPages.Add(this.AccountPage);
            this.footerPages.Add(this.SettingsPage);

            this.CurrentPage = this.AccountPage;
        }

        public ObservableCollection<IPageVM> HeaderPages => this.headerPages;

        public ObservableCollection<IPageVM> FooterPages => this.footerPages;

        public LoginPageVm AccountPage { get; private set; }

        public SettingsPageVm SettingsPage { get; private set; }
    }
}
