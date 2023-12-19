// -----------------------------------------------------------------------
// <copyright file="ValidatedTextVM.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class ValidatedTextVM : ObservableObject
    {
        private readonly Func<string, string> validationFunc;
        
        [ObservableProperty]
        private string text = string.Empty;
        
        [ObservableProperty]
        private bool isEnabled = true;

        [ObservableProperty]
        private string error = string.Empty;

        public ValidatedTextVM(
            Func<string, string> validationFunc)
        {
            this.validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
        }

        public bool IsInvalid => !string.IsNullOrWhiteSpace(this.Error);

        partial void OnTextChanged(string value)
        {
            this.Error = this.validationFunc(value) ?? string.Empty;
        }
    }
}
