// -----------------------------------------------------------------------
// <copyright file="SettingsPage.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;
    using Windows.System;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public SettingsPageVm ViewModel { get; } = App.ViewModelResolver.Resolve<SettingsPageVm>();

        private void OpenAppDataFolderButton_Click(object sender, RoutedEventArgs e)
        {
            _ = Launcher.LaunchFolderPathAsync(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }
    }
}
