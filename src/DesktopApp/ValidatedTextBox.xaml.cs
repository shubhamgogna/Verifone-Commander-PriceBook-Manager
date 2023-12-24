// -----------------------------------------------------------------------
// <copyright file="ValidatedTextBox.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class ValidatedTextBox : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
            nameof(Error),
            typeof(string),
            typeof(ValidatedTextBox),
            new PropertyMetadata(default(string)));

        public ValidatedTextBox()
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

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
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
