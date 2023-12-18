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
        private readonly Action afterTextChangeFunc;

        [ObservableProperty]
        private string text;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string error;

        public ValidatedTextVm(
            Func<string, string> validationFunc)
            : this(validationFunc, string.Empty)
        {
        }

        public ValidatedTextVm(
            Func<string, string> validationFunc,
            string initialValue)
            : this(validationFunc, initialValue, null)
        {
        }

        public ValidatedTextVm(
            Func<string, string> validationFunc,
            string initialValue,
            Action afterTextChangeFunc)
        {
            this.validationFunc = validationFunc;
            this.afterTextChangeFunc = afterTextChangeFunc;

            this.text = initialValue;
            this.error = this.validationFunc?.Invoke(this.text) ?? string.Empty;
        }

        public bool HasError => !string.IsNullOrEmpty(this.Error);

        public bool HasError_Revalidate()
        {
            // This method is useful if the validation function conditions changed
            this.Error = this.validationFunc?.Invoke(this.Text) ?? string.Empty;
            return this.HasError;
        }

        partial void OnTextChanged(string value)
        {
            this.Error = this.validationFunc?.Invoke(value) ?? string.Empty;

            this.afterTextChangeFunc?.Invoke();
        }
    }
}
