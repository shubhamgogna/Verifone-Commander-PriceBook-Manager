// -----------------------------------------------------------------------
// <copyright file="IUiThreadDispatcher.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using System.Threading.Tasks;

    public interface IUiThreadDispatcher
    {
        Task DispatchAsync(Action action);
    }
}
