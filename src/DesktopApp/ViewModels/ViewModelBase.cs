// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;

    public abstract class ViewModelBase : ObservableObject
    {
        protected ViewModelBase(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger)
        {
            this.UiThreadDispatcher = uiThreadDispatcher ?? throw new ArgumentNullException(nameof(uiThreadDispatcher));
            this.Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IUiThreadDispatcher UiThreadDispatcher { get; }

        protected IMessenger Messenger { get; }

        protected ILogger Logger { get; }

        protected Task DispatchOnUiThreadAsync(Action action)
        {
            return this.UiThreadDispatcher.DispatchAsync(action);
        }
    }
}
