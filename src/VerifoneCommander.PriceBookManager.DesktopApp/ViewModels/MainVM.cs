// -----------------------------------------------------------------------
// <copyright file="MainVM.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;

    public class MainVM : ObservableObject
    {
        private readonly AccountPageVM accountPage = new AccountPageVM();
        private readonly ObservableCollection<IPageVM> pages = new ObservableCollection<IPageVM>();

        public MainVM() 
        { 
            this.pages.Add(this.accountPage);
        }

        public ObservableCollection<IPageVM> Pages => this.pages;
    }
}
