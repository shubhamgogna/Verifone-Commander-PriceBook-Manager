// -----------------------------------------------------------------------
// <copyright file="ValidatedTextBox.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public sealed partial class ValidatedTextBox : UserControl
    {
        public ValidatedTextBox()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ValidatedTextVM),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(ValidatedTextVM)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public ValidatedTextVM ViewModel
        {
            get => (ValidatedTextVM)this.GetValue(ViewModelProperty);
            set => this.SetValue(ViewModelProperty, value);
        }

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        public string Placeholder
        {
            get => (string)this.GetValue(PlaceholderProperty);
            set => this.SetValue(PlaceholderProperty, value);
        }
    }
}
