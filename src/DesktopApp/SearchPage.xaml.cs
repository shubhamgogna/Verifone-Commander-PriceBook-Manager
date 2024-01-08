// -----------------------------------------------------------------------
// <copyright file="SearchPage.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }

        public SearchPageVm ViewModel { get; } = App.ViewModelResolver.Resolve<SearchPageVm>();
    }
}
