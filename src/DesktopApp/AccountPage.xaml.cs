// -----------------------------------------------------------------------
// <copyright file="AccountPage.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class AccountPage : Page
    {
        public AccountPage()
        {
            this.InitializeComponent();
        }

        public AccountPageVm ViewModel { get; } = App.ViewModelResolver.Resolve<AccountPageVm>();
    }
}
