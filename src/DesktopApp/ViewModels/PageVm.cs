// -----------------------------------------------------------------------
// <copyright file="PageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;

    public abstract class PageVm : ViewModelBase
    {
        protected PageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger)
            : base(uiThreadDispatcher, messenger, logger)
        {
        }

        public abstract string Name { get; }

        public abstract int SymbolCode { get; }

        public InfoBarVm InfoBar { get; } = new InfoBarVm();

        protected void SetInfoBar(
            InfoBarSeverity severity,
            string message)
        {
            this.InfoBar.IsOpen = false;
            this.InfoBar.Severity = severity;
            this.InfoBar.Message = message;
            this.InfoBar.IsOpen = true;
        }
    }
}
