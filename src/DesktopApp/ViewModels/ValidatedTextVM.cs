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
        private string text = string.Empty;

        [ObservableProperty]
        private string error = string.Empty;

        public ValidatedTextVm(
            Func<string, string> validationFunc)
        {
            this.validationFunc = validationFunc;
        }

        partial void OnTextChanged(string value)
        {
            this.Error = this.validationFunc?.Invoke(value) ?? string.Empty;
        }
    }
}
