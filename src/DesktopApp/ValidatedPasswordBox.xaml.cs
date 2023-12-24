// -----------------------------------------------------------------------
// <copyright file="ValidatedPasswordBox.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class ValidatedPasswordBox : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidatedPasswordBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(ValidatedPasswordBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            nameof(Password),
            typeof(string),
            typeof(ValidatedPasswordBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
            nameof(Error),
            typeof(string),
            typeof(ValidatedPasswordBox),
            new PropertyMetadata(default(string)));

        public ValidatedPasswordBox()
        {
            this.InitializeComponent();
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

        public string Password
        {
            get => (string)this.GetValue(PasswordProperty);
            set => this.SetValue(PasswordProperty, value);
        }

        public string Error
        {
            get => (string)this.GetValue(ErrorProperty);
            set
            {
                this.SetValue(ErrorProperty, value);
                this.ErrorTextBlock.Visibility = string.IsNullOrWhiteSpace(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
