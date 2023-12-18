// -----------------------------------------------------------------------
// <copyright file="EditPage.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
        }

        public EditPageVm ViewModel { get; } = App.ViewModelResolver.Resolve<EditPageVm>();

        private void EanTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.EanTextBox.SelectAll();
        }

        private void Button_Click_CloseFlyout(object sender, RoutedEventArgs e)
        {
            if (this.DeleteButton.Flyout is Flyout f)
            {
                f.Hide();
            }
        }
    }
}
