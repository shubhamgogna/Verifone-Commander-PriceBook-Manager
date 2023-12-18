// -----------------------------------------------------------------------
// <copyright file="BulkOperationsPage.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class BulkOperationsPage : Page
    {
        public BulkOperationsPage()
        {
            this.InitializeComponent();
        }

        public BulkOperationsPageVm ViewModel { get; } = App.ViewModelResolver.Resolve<BulkOperationsPageVm>();
    }
}
