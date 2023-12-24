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
        private readonly MainNavigationVm mainNavigationVm = new MainNavigationVm();

        public T Resolve<T>()
            where T : ObservableObject
        {
            if (typeof(T) == typeof(MainNavigationVm))
            {
                return this.mainNavigationVm as T;
            }
            else if (typeof(T) == typeof(LoginPageVm))
            {
                return this.mainNavigationVm.AccountPage as T;
            }
            else if (typeof(T) == typeof(SettingsPageVm))
            {
                return this.mainNavigationVm.SettingsPage as T;
            }

            throw new ArgumentOutOfRangeException(nameof(T), typeof(T).FullName, "Unknown type");
        }
    }
}
