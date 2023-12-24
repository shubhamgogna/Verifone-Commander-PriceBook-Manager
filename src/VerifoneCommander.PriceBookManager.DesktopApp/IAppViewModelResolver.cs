// -----------------------------------------------------------------------
// <copyright file="IAppViewModelResolver.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public interface IAppViewModelResolver
    {
        T Resolve<T>()
            where T : ObservableObject;
    }
}
