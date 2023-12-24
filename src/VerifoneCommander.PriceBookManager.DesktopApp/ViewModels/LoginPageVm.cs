// -----------------------------------------------------------------------
// <copyright file="LoginPageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;

    public class LoginPageVm : ObservableObject, IPageVM
    {
        private readonly Settings settings;

        public LoginPageVm()
        {
            this.Username = new ValidatedTextVm(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return "Empty username not allowed";
                }

                // Valid. Update in settings.
                this.settings.Username = value;
                return string.Empty;
            });

            this.Password = new ValidatedTextVm(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return "Empty username not allowed";
                }

                // Valid
                return string.Empty;
            });
        }

        public string Name => "Login";

        // Contact
        public int SymbolCode => 0xE77B;

        public ValidatedTextVm Username { get; }

        public ValidatedTextVm Password { get; }
    }
}
