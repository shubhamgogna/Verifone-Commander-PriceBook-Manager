// -----------------------------------------------------------------------
// <copyright file="MainNavigationView.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class MainNavigationView : UserControl
    {
        public MainNavigationView()
        {
            this.InitializeComponent();
        }

        public MainNavigationVm ViewModel { get; } = App.ViewModelResolver.Resolve<MainNavigationVm>();

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (args.SelectedItem.GetType() == typeof(AccountPageVm))
            {
                this.NavViewFrame.Navigate(typeof(AccountPage));
            }
            else if (args.SelectedItem.GetType() == typeof(SettingsPageVm))
            {
                this.NavViewFrame.Navigate(typeof(SettingsPage));
            }
        }
    }
}
