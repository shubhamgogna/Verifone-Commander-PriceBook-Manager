// -----------------------------------------------------------------------
// <copyright file="ValidatedTextVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class ValidatedTextVm : ObservableObject
    {
        private readonly Func<string, string> validationFunc;

        [ObservableProperty]
        private string text;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string error;

        public ValidatedTextVm(
            Func<string, string> validationFunc)
            : this(string.Empty, validationFunc)
        {
        }

        public ValidatedTextVm(
            string initialValue,
            Func<string, string> validationFunc)
        {
            this.validationFunc = validationFunc;

            this.Text = initialValue;
        }

        public bool HasError => !string.IsNullOrEmpty(this.Error);

        partial void OnTextChanged(string value)
        {
            this.Error = this.validationFunc?.Invoke(value) ?? string.Empty;
        }
    }
}
