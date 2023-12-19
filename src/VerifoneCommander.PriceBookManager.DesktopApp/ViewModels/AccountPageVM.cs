// -----------------------------------------------------------------------
// <copyright file="AccountPageVM.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public class AccountPageVM : ObservableObject, IPageVM
    {
        public AccountPageVM()
        {
            this.Hostname = new ValidatedTextVM(_ => string.Empty);
            this.Username = new ValidatedTextVM(_ => string.Empty);
        }

        public string Name => "Account";

        public int SymbolCode => 57661; // Contact

        public ValidatedTextVM Hostname { get; }

        public ValidatedTextVM Username { get; }
    }
}
