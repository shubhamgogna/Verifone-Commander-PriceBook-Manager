// -----------------------------------------------------------------------
// <copyright file="AppViewModelResolver.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    public class AppViewModelResolver : IAppViewModelResolver
    {
        private readonly MainNavigationVm mainNavigationVm;

        public AppViewModelResolver(
            MainNavigationVm mainNavigationVm)
        {
            this.mainNavigationVm = mainNavigationVm ?? throw new ArgumentNullException(nameof(mainNavigationVm));
        }

        public T Resolve<T>()
            where T : ObservableObject
        {
            if (typeof(T) == typeof(MainNavigationVm))
            {
                return this.mainNavigationVm as T;
            }
            else if (typeof(T) == typeof(AccountPageVm))
            {
                return this.mainNavigationVm.AccountPage as T;
            }
            else if (typeof(T) == typeof(BulkOperationsPageVm))
            {
                return this.mainNavigationVm.BulkOperationsPage as T;
            }
            else if (typeof(T) == typeof(EditPageVm))
            {
                return this.mainNavigationVm.EditPage as T;
            }
            else if (typeof(T) == typeof(SearchPageVm))
            {
                return this.mainNavigationVm.SearchPlusPage as T;
            }
            else if (typeof(T) == typeof(SettingsPageVm))
            {
                return this.mainNavigationVm.SettingsPage as T;
            }

            throw new ArgumentOutOfRangeException(nameof(T), typeof(T).FullName, "Unknown type");
        }
    }
}
